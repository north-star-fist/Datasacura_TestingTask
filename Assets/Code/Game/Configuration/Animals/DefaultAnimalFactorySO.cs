using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Datasacura.TestTask.ZooWorld.Config
{
    [CreateAssetMenu(fileName = "Animal", menuName = "Zoo World/Animals/Geneic Animal")]
    public class DefaultAnimalFactorySO : AAnimalFactorySO
    {
        [SerializeField]
        private AAnimal _animalPrefab;

        [NonSerialized]
        private readonly Dictionary<IAnimal, GameObject> _spawnedObjects = new();

        [NonSerialized]
        private IObjectPool<GameObject> _pool;

        public override IAnimal GetAnimal(IGameSceneContext sceneContext, Vector3 newLocation, Quaternion newRotation)
        {
            AAnimal animal = Spawn(sceneContext, newLocation, newRotation);
            _spawnedObjects[animal] = animal.gameObject;
            return animal;
        }

        public override bool ReleaseAnimal(IGameSceneContext sceneContext, IAnimal animal)
        {
            if (_spawnedObjects.TryGetValue(animal, out var go))
            {
                if (go != null)
                {
                    // It can be destroyed with the scene, so better to check for null
                    GetPool(sceneContext).Release(go);
                }
                return true;
            }
            return false;
        }

        private AAnimal Spawn(IGameSceneContext gameScene, Vector3 newLocation, Quaternion newRotation)
        {
            var instance = GetPool(gameScene).Get();
            instance.transform.SetPositionAndRotation(newLocation, newRotation);
            return instance.GetComponent<AAnimal>();
        }

        private IObjectPool<GameObject> GetPool(IGameSceneContext gameScene)
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<GameObject>(
                    createFunc: () => GameObject.Instantiate(_animalPrefab, gameScene.AnimalParent).gameObject,
                    actionOnGet: (obj) => obj.SetActive(true),
                    actionOnRelease: (obj) => obj.SetActive(false),
                    actionOnDestroy: (obj) =>
                    {
                        _spawnedObjects.Remove(obj.GetComponent<IAnimal>());
                        Destroy(obj);
                    });
            }
            return _pool;
        }
    }
}
