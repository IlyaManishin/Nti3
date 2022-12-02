using UnityEngine;

public class CameraFollower : MonoBehaviour {
    [SerializeField] private Transform _cameraFollower;
    [SerializeField] private Vector3 _offsetCamera;
    [SerializeField] private float _smoothing;

    private void FixedUpdate() {
        move();
    }
    
    private void move() {
        transform.position = Vector3.Lerp(transform.position, _cameraFollower.position + _offsetCamera, Time.fixedDeltaTime * _smoothing);
    }
}
