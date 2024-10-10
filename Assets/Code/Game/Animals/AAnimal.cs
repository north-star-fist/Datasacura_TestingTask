using Datasakura.TestTask.ZooWorld.Util;
using R3;
using UnityEngine;

namespace Datasakura.TestTask.ZooWorld
{
    public abstract class AAnimal : MonoBehaviour, IAnimal
    {
        [SerializeField, Tooltip("Within this angle animal is treated as directed to a destination")]
        protected int _directionAngleThreshold = 2;

        public abstract bool IsPredator { get; }

        public Vector3 Position => transform.position;

        public abstract Vector3 Tick(float deltaTime);

        public Observable<(IAnimal, IAnimal)> OnAnimalCollision => _onAnimalCollision;

        private readonly Subject<(IAnimal, IAnimal)> _onAnimalCollision = new Subject<(IAnimal, IAnimal)>();
        protected ReactiveProperty<Vector3?> _destinationRx = new ReactiveProperty<Vector3?>(null);

        public void SetDestination(Vector3? destination)
        {
            _destinationRx.OnNext(destination);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterInternal(collision);

            if (gameObject == null || collision.gameObject == null) return;
            if (this.gameObject.CompareTag(collision.gameObject.tag))
            {
                IAnimal animal = collision.gameObject.GetComponent<IAnimal>();
                // seems to be animal
                _onAnimalCollision.OnNext((this, animal));
            }
        }

        protected virtual void OnCollisionEnterInternal(Collision collision)
        {
        }

        protected virtual bool IsDestinationAhead()
        {
            if (!_destinationRx.Value.HasValue)
            {
                return false;
            }

            Vector3 destination = _destinationRx.Value.Value;
            var distDir = (destination - transform.position).WithY(0);
            float angle = Mathf.Abs(Vector3.Angle(transform.forward, distDir));
            return angle < _directionAngleThreshold;
        }
    }
}
