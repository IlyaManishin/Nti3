using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerMove : MonoBehaviour {
    private Rigidbody2D _rigidbody2D;
    private Controls _playerInput;
    private bool _isSprinting = false;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintSpeed;

    public void OnSprint(InputAction.CallbackContext context) => _isSprinting = context.ReadValueAsButton();

    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerInput = new Controls();
    }

    private void OnEnable() {
        _playerInput.Enable();
    }

    private void OnDisable() {
        _playerInput.Disable();
    }

    private void FixedUpdate() {
        _rigidbody2D.velocity = _playerInput.Player.Move.ReadValue<Vector2>() * (_isSprinting? _sprintSpeed : _moveSpeed) * Time.fixedDeltaTime;
    }

}
