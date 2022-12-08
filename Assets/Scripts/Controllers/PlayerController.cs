using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.InputSystem;

namespace TheGameIdk.Controllers {
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour {
        public static Rigidbody2D instance { get; private set; }

        [Header("Movement")]
        [SerializeField] private float _movementAcceleration = 2f;
        [SerializeField] private float _movementSpeed = 12f;
        [SerializeField] private float _sprintAcceleration = 6f;
        [SerializeField] private float _sprintSpeed = 32f;

        private Rigidbody2D _rigidbody;

        private Vector2 _movement;
        private bool _sprinting;
        private bool _stoppedSprinting;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
            instance = _rigidbody;
        }

        private void FixedUpdate() {
            float speed = _sprinting ? _sprintSpeed : _movementSpeed;
            float acceleration = _sprinting ? _sprintAcceleration : _movementAcceleration;
            if(_stoppedSprinting) {
                _rigidbody.velocity -= _movement * (_sprintSpeed - _movementSpeed);
                _stoppedSprinting = false;
            }
            _rigidbody.AddEntityForce(_movement * (acceleration * Time.deltaTime), speed);
        }

        [UsedImplicitly]
        public void OnMove(InputAction.CallbackContext context) => _movement = context.ReadValue<Vector2>();

        [UsedImplicitly]
        public void OnSprint(InputAction.CallbackContext context) {
            _sprinting = context.ReadValueAsButton();
            if(_sprinting)
                return;
            _stoppedSprinting = true;
        }
    }
}
