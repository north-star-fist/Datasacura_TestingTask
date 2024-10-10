using Cysharp.Threading.Tasks;
using Datasacura.TestTask.ZooWorld.Util;
using R3;
using UnityEngine;

namespace Datasacura.TestTask.ZooWorld
{
    public class Snake : AAnimal
    {
        [Header("Snake settings")]
        [SerializeField, Tooltip("Interval in seconds wihthin which the snake is moving")]
        private float _moveTime = 3f;
        [SerializeField, Tooltip("Minimum delay in seconds between locomotions")]
        private float _idleTimeMin = 1f;
        [SerializeField, Tooltip("Maximum delay in seconds between locomotions")]
        private float _idleTimeMax = 2f;
        [SerializeField, Tooltip("Movement speed in meters per second")]
        private float _movementSpeed = 1f;
        [SerializeField, Tooltip("Rotation speed in degrees per second")]
        private float _rotationSpeed = 1f;

        private float _moveTimer;
        private float _idleTimer;
        private Rigidbody _rigidBody;

        private float _idleTime;
        private int _rotationDir = 1;
        private bool _isMoving;
        private Vector3 _velocity;

        public override bool IsPredator => true;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _destinationRx.DistinctUntilChanged().Subscribe(HandleNewDestination).AddTo(this);
            ResetIdleTime();
        }

        public override Vector3 Tick(float deltaTime)
        {
            _rigidBody.velocity = _velocity.WithY(_rigidBody.velocity.y);
            if (_isMoving)
            {
                move();
            }
            else
            {
                rotate();
            }
            return transform.position;

            void move()
            {
                _moveTimer += deltaTime;
                if (_moveTimer >= _moveTime)
                {
                    StopMotion();
                }
            }

            void rotate()
            {
                if (!_destinationRx.Value.HasValue)
                {
                    _idleTimer += deltaTime;
                }
                if (_idleTimer >= _idleTime)
                {
                    StartMotion();
                }
                else
                {
                    Quaternion newRotation = Quaternion.Euler(
                        transform.rotation.eulerAngles + new Vector3(0, _rotationDir * _rotationSpeed, 0)
                    );
                    _rigidBody.MoveRotation(newRotation);
                }
                if (IsDestinationAhead())
                {
                    StartMotion();
                }
            }
        }

        protected override void OnCollisionEnterInternal(Collision collision)
        {
            base.OnCollisionEnterInternal(collision);

            if (IsGround(collision.collider))
            {
                StopMotion();
            }
        }

        void HandleNewDestination(Vector3? destination)
        {
            if (!destination.HasValue)
            {
                // We obviously want to stop motion directed to old destination, so let's stop
                StopMotion();
            }
        }

        private void StartMotion()
        {
            _velocity = transform.forward * _movementSpeed;
            _isMoving = true;
            ResetIdleTime();
            _idleTimer = 0;
        }

        private void StopMotion()
        {
            _velocity = Vector3.zero;
            _moveTimer = 0;
            _idleTimer = 0;
            _isMoving = false;
            _rotationDir = Random.value >= 0.5f ? 1 : -1;
        }

        private bool IsGround(Collider collider) => !collider.gameObject.CompareTag(gameObject.tag);

        private void ResetIdleTime()
        {
            _idleTime = Random.Range(_idleTimeMin, _idleTimeMax);
        }
    }
}
