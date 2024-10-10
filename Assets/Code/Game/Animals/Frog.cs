using System;
using R3;
using UnityEngine;

namespace Datasakura.TestTask.ZooWorld
{
    [RequireComponent(typeof(Rigidbody))]
    public class Frog : AAnimal
    {
        [Header("Frog settings")]
        [SerializeField, Tooltip("Delay between jumps in seconds")]
        private float _jumpTime = 3f;
        [SerializeField, Tooltip("Jump distance in meters")]
        private float _jumpDistance = 3;
        [SerializeField, Tooltip("Jump height in meters")]
        private float _jumpHeight = 3f;
        [SerializeField, Tooltip("Rotation speed in degrees")]
        private float _rotationSpeed = 1f;

        private float _jumpTimer;
        private Rigidbody _rigidBody;
        private Vector3 _jumpVelocity;
        private int _rotationDir = 1;
        private bool _isGrounded = true;

        public override bool IsPredator => false;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _jumpVelocity = CalculateJumpVelocity();
            _destinationRx.DistinctUntilChanged().Subscribe(HandleNewDestination).AddTo(this);
        }

        public override Vector3 Tick(float deltaTime)
        {
            if (_isGrounded)
            {
                rotate();
            }
            if (!_destinationRx.Value.HasValue)
            {
                _jumpTimer += deltaTime;
            }
            if (isTimeToJump())
            {
                _jumpTimer = 0;
                Jump();
            }
            return transform.position;

            void rotate()
            {
                Quaternion newRotation = Quaternion.Euler(
                    transform.rotation.eulerAngles + new Vector3(0, _rotationDir * _rotationSpeed, 0)
                );
                _rigidBody.MoveRotation(newRotation);
            }

            bool isTimeToJump()
            {
                return _isGrounded && (_jumpTimer >= _jumpTime || IsDestinationAhead());
            }
        }

        protected override void OnCollisionEnterInternal(Collision collision)
        {
            base.OnCollisionEnterInternal(collision);

            if (IsGround(collision.collider))
            {
                _isGrounded = true;
                _rotationDir = UnityEngine.Random.value >= 0.5f ? 1 : -1;
            }
        }

        private void Jump()
        {
            _isGrounded = false;
            Vector3 jumpVel = transform.rotation * _jumpVelocity;
            _rigidBody.AddForce(jumpVel, ForceMode.VelocityChange);
        }

        void HandleNewDestination(Vector3? destination)
        {
            // We do not care about destination here. We check it in Tick() method
        }

        private bool IsGround(Collider collider) => !collider.gameObject.CompareTag(gameObject.tag);

        private Vector3 CalculateJumpVelocity()
        {
            // h = g * th^2 / 2
            float g = Math.Abs(Physics.gravity.y);
            float th = Mathf.Sqrt(2 * _jumpHeight / g);
            // vv * th = g * th^2 / 2
            float vv = g * th / 2;
            // d = vh * t
            // t = 2 * th
            float vh = _jumpDistance / (2 * th);
            return new Vector3(0, vv, vh);
        }
    }
}
