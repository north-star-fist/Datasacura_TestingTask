using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Config;
using Datasacura.TestTask.ZooWorld.Util;
using Datasacura.TestTask.ZooWorld.Visuals;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Datasacura.TestTask.ZooWorld
{
    /// <summary>
    /// Scene object that manages the game.
    /// </summary>
    public class GameManager : MonoBehaviour, IGameManager, IGameSceneContext
    {
        [SerializeField]
        private Bounds _boundingBox;
        [
            SerializeField,
            Tooltip("When an animal is out of this box, it is destroyed. Must be larger than Bounding Box")
        ]
        private Bounds _existenceBox;
        [SerializeField]
        private Transform _animalParent;
        [SerializeField, Tooltip("Kill notificatin prefab (\"Tasty!\")")]
        private KillNotification _killNotification;
        [SerializeField, Tooltip("Time the notification should be shown for (in seconds)")]
        private float _killNotificationTime = 1;

        public GameStats GameStats => _gameStats;
        private GameStats _gameStats = default;

        public Transform AnimalParent => _animalParent;

        public Observable<IAnimal> OnAnimalDead => _onAnimalDead;
        private readonly Subject<IAnimal> _onAnimalDead = new Subject<IAnimal>();

        private IGameSceneContext _sceneContext;
        private ObjectPool<KillNotification> _killNotifPool;
        private readonly List<IAnimalFactory> _animalFactories = new();
        private readonly Dictionary<IAnimal, IDisposable> _collisionSubscriptions = new();

        // animals and their factories.
        private readonly Dictionary<IAnimal, IAnimalFactory> _animalFactoryMap = new();

        [Inject]
        public void Init(IGameSceneContext sceneContext)
        {
            _sceneContext = sceneContext;
        }

        /// <summary>
        /// Starts new game with specified animal database.
        /// </summary>
        public async UniTask StartGameAsync(LevelSO level, CancellationToken cancellationToken)
        {
            // Clear previous game set
            Clear();

            // init
            initAnimalFactories(level);
            if (_animalFactories.Count == 0)
            {
                Debug.LogWarning("Animal database is empty! No animals to spawn");
                return;
            }

            // loop
            // TODO: cache some pauses into array and resuse them
            while (!cancellationToken.IsCancellationRequested)
            {
                float randomDelay = UnityEngine.Random.Range(level.AnimalSpawnMinDelay, level.AnimalSpawnMaxDelay);
                await UniTask.Delay(TimeSpan.FromSeconds(randomDelay));

                SpawnAnimal(getRandomPosition());
            }

            Vector3 getRandomPosition()
            {
                var randomExtents = _boundingBox.extents;
                randomExtents.Scale(UnityEngine.Random.onUnitSphere);
                randomExtents.y = -_boundingBox.extents.y;
                Vector3 position = _boundingBox.center + randomExtents;
                return position;
            }

            void initAnimalFactories(LevelSO level)
            {
                foreach (var animFactory in level.Animals.Animals)
                {
                    if (animFactory == null)
                    {
                        Debug.LogWarning("Animal database contains null!");
                        continue;
                    }
                    _animalFactories.Add(animFactory);
                }
            }
        }

        readonly List<IAnimal> _animalsFlewAway = new List<IAnimal>();
        private void Update()
        {
            _animalsFlewAway.Clear();
            foreach (var animal in _animalFactoryMap.Keys)
            {
                if (IsOutOfBounds(animal.Position, _existenceBox))
                {
                    // the animal flew away too far
                    _animalsFlewAway.Add(animal);
                }
                else
                {
                    Vector3 boxFloorCenter = _boundingBox.center - new Vector3(0, _boundingBox.extents.y, 0);
                    animal.SetDestination(IsOutOfBounds(animal.Position, _boundingBox) ? transform.position + boxFloorCenter : null);
                }
                animal.Tick(Time.deltaTime);
            }
            foreach (var animal in _animalsFlewAway)
            {
                KillAnimal(animal, false);
            }
        }

        private void OnDestroy() => Clear();


        private void HandleAnimalCollision((IAnimal a1, IAnimal a2) aCol)
        {
            if (!_animalFactoryMap.ContainsKey(aCol.a1) || !_animalFactoryMap.ContainsKey(aCol.a2))
            {
                return;
            }

            IAnimal victim = null;
            if (aCol.a1.IsPredator || aCol.a2.IsPredator)
            {
                // Ok, someone should go
                if (!aCol.a1.IsPredator)
                {
                    victim = aCol.a1;
                }
                else if (!aCol.a2.IsPredator)
                {
                    victim = aCol.a2;
                }
                else
                {
                    // clash
                    bool firstOneChosen = UnityEngine.Random.value >= 0.5f;
                    victim = firstOneChosen ? aCol.a1 : aCol.a2;
                }
            }
            if (victim == null)
            {
                return;
            }

            KillAnimal(victim);
            var hunter = victim == aCol.a1 ? aCol.a2 : aCol.a1;
            ShowNotification(hunter.Position);
        }

        private void SpawnAnimal(Vector3 position)
        {
            IAnimalFactory animalFactory = GetRandomAnimalFactory();
            IAnimal animal = animalFactory.GetAnimal(
                _sceneContext,
                position,
                Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0)
            );
            _collisionSubscriptions[animal] = animal.OnAnimalCollision.Subscribe(HandleAnimalCollision);
            _animalFactoryMap[animal] = animalFactory;

            UpdateGameStats(animal.IsPredator, +1, 0);
        }

        private void KillAnimal(IAnimal victim, bool updateStats = true)
        {
            if (_animalFactoryMap.TryGetValue(victim, out var factory))
            {
                factory.ReleaseAnimal(_sceneContext, victim);
                if (_collisionSubscriptions.Remove(victim, out var sub))
                {
                    sub.Dispose();
                }
                _animalFactoryMap.Remove(victim);

                if (updateStats)
                {
                    UpdateGameStats(victim.IsPredator, -1, +1);
                }
                _onAnimalDead.OnNext(victim);
            }
        }

        private void ShowNotification(Vector3 position)
        {
            KillNotification notification = GetNotificationInstance();
            notification.transform.position = position;
            notification.transform.SetPositionAndRotation(position, Quaternion.Euler(0, UnityEngine.Random.value * 360, 0));
            notification.Show(_killNotificationTime).ContinueWith(() => ReleaseNotificationInstance(notification)).Forget();
        }

        private void UpdateGameStats(bool isPredator, int aliveDelta, int deadDelta)
        {
            if (isPredator)
            {
                _gameStats = _gameStats
                    .WithAlivePredators(_gameStats.AlivePredators + aliveDelta)
                    .WithDeadPredators(_gameStats.DeadPredators + deadDelta);
            }
            else
            {
                _gameStats = _gameStats
                    .WithAlivePrey(_gameStats.AlivePrey + aliveDelta)
                    .WithDeadPrey(_gameStats.DeadPrey + deadDelta);
            }
        }

        private KillNotification GetNotificationInstance() => GetKillNotificationPool().Get();

        private void ReleaseNotificationInstance(KillNotification notification)
        {
            GetKillNotificationPool().Release(notification);
        }

        private IObjectPool<KillNotification> GetKillNotificationPool()
        {
            if (_killNotifPool == null)
            {
                _killNotifPool = new ObjectPool<KillNotification>(
                    createFunc: () => Instantiate(_killNotification, transform),
                    actionOnGet: null,
                    actionOnRelease: null,
                    actionOnDestroy: (obj) => GameObject.Destroy(_killNotification.gameObject)
                );
            }
            return _killNotifPool;
        }

        private void Clear()
        {
            _animalFactories.Clear();
            _gameStats = new GameStats();
            foreach ((var animal, var factory) in _animalFactoryMap)
            {
                factory.ReleaseAnimal(_sceneContext, animal);
            }
            _animalFactoryMap.Clear();
        }

        private IAnimalFactory GetRandomAnimalFactory()
        {
            return _animalFactories[UnityEngine.Random.Range(0, _animalFactories.Count)];
        }

        private bool IsOutOfBounds(Vector3 position, Bounds bounds) => !bounds.Contains(position - transform.position);

        private void OnValidate()
        {
            if (_existenceBox.min.x > _boundingBox.min.x)
            {
                _existenceBox.min = _existenceBox.min.WithX(_boundingBox.min.x);
            }
            if (_existenceBox.max.x < _boundingBox.max.x)
            {
                _existenceBox.max = _existenceBox.max.WithX(_boundingBox.max.x);
            }
            if (_existenceBox.min.y > _boundingBox.min.y)
            {
                _existenceBox.min = _existenceBox.min.WithY(_boundingBox.min.y);
            }
            if (_existenceBox.max.y < _boundingBox.max.y)
            {
                _existenceBox.max = _existenceBox.max.WithY(_boundingBox.max.y);
            }
            if (_existenceBox.min.z > _boundingBox.min.z)
            {
                _existenceBox.min = _existenceBox.min.WithZ(_boundingBox.min.z);
            }
            if (_existenceBox.max.z < _boundingBox.max.z)
            {
                _existenceBox.max = _existenceBox.max.WithZ(_boundingBox.max.z);
            }
        }
    }
}
