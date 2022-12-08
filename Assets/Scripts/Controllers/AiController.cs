using UnityEngine;

using Random = UnityEngine.Random;

namespace TheGameIdk.Controllers {
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AiController : MonoBehaviour {
        protected abstract Rigidbody2D target { get; }

        [Header("Movement")]
        [SerializeField] private float _movementAcceleration = 2f;
        [SerializeField] protected float movementSpeed = 3f;
        [SerializeField] private float _steerSpeed = 4f;

        [Header("AI")]
        [SerializeField] private GlobalControllerSettings _globalSettings;
        [SerializeField] private int _rayCount = 16;
        [SerializeField] private float _rayDistance = 3f;
        [SerializeField] private float _rayRadius = 0.9f;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private float _targetReachedDistance = 3f;

        [Header("Pursue AI")]
        [SerializeField] private float _pursuePredictionDistance = 0.3f;

        [Header("Wander AI")]
        [SerializeField] private float _nextRandomPointMinDistance = 0.5f;
        [SerializeField] private float _nextRandomPointMaxDistance = 6f;
        [SerializeField] private float _wanderRaycastDistanceOffset = 0.2f;
        [SerializeField] private int _reachedCountUntilNewDirection = 6;

        private Rigidbody2D _rigidbody;

        private Vector2 _targetPosition;
        private Vector2 _directionToTarget;

        private Vector2[] _rayDirections;
        private RaycastHit2D[] _hits;
        private bool _canMoveInAllDirections = true;
        private Vector2 _targetMovement;
        private Vector2 _movement;

        private int _reachedTargetsInWander;
        private Vector2 _wanderDirection;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();

            _rayDirections = new Vector2[_rayCount];
            _hits = new RaycastHit2D[_rayCount];
            for(int i = 0; i < _rayCount; i++) {
                float angle = 360f / _rayCount * i;
                _rayDirections[i] = Util.DegreesToVector(angle);
            }
        }

        private void FixedUpdate() {
            Vector2 position = transform.position;

            UpdateState(position);

            Vector2 vectorToTarget = _targetPosition - position;
            float distanceToTarget = vectorToTarget.magnitude;
            _directionToTarget = vectorToTarget.normalized;

            _canMoveInAllDirections = true;
            for(int i = 0; i < _rayCount; i++) {
                _hits[i] = Physics2D.CircleCast(position, _rayRadius, _rayDirections[i], _rayDistance,
                    _globalSettings.wallLayerMask);
                if(_hits[i])
                    _canMoveInAllDirections = false;
            }

            bool canSeeTarget = !Physics2D.Raycast(position, _directionToTarget, distanceToTarget,
                _globalSettings.wallLayerMask);
            _canMoveInAllDirections |= canSeeTarget;
            if(_canMoveInAllDirections) {
                _targetMovement = _directionToTarget;
                ProcessMovement();
                return;
            }

            float angleToTarget = Vector2.SignedAngle(Vector2.right, _directionToTarget);

            ChooseDirection(angleToTarget);

            ProcessMovement();
        }

        private void UpdateState(Vector2 position) {
            if(target) {
                Vector2 directionToTarget = (target.position - position).normalized;
                RaycastHit2D targetHit =
                    Physics2D.Raycast(position, directionToTarget, float.PositiveInfinity,
                        _globalSettings.wallLayerMask | _targetLayerMask);
                bool canSeeTarget = targetHit && targetHit.transform == target.transform;

                if(canSeeTarget) {
                    _targetPosition = targetHit.point; // seek
                    if(targetHit.distance > _targetReachedDistance)
                        _targetPosition += target.velocity * _pursuePredictionDistance; // pursue
                    return;
                }
            }

            // wander
            float distanceToTarget = (_targetPosition - position).magnitude;
            bool targetReached = distanceToTarget <= _targetReachedDistance;

            if(!targetReached)
                return;
            _reachedTargetsInWander++;
            if(_reachedTargetsInWander >= _reachedCountUntilNewDirection) {
                _wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                _reachedTargetsInWander = 0;
            }
            float distance = Random.Range(_nextRandomPointMinDistance, _nextRandomPointMaxDistance);
            RaycastHit2D hit = Physics2D.Raycast(_targetPosition, _wanderDirection, distance, _globalSettings.wallLayerMask);
            _targetPosition += hit ? _wanderDirection * (hit.distance - _wanderRaycastDistanceOffset) :
                _wanderDirection * distance;
        }

        private void ChooseDirection(float angleToTarget) {
            float minAngle = float.PositiveInfinity;
            _targetMovement = Vector2.zero;
            for(int i = 0; i < _rayCount; i++) {
                if(_hits[i])
                    continue;

                Vector2 direction = _rayDirections[i];
                float angle = Vector2.SignedAngle(Vector2.right, direction);
                angle = Mathf.Abs(Mathf.DeltaAngle(angle, angleToTarget));

                if(angle >= minAngle)
                    continue;
                minAngle = angle;
                _targetMovement = direction;
            }
        }

        private void ProcessMovement() {
            _movement += (_targetMovement - _movement) * (_steerSpeed * Time.deltaTime);
            _movement.Normalize();
            _rigidbody.AddEntityForce(_movement * (_movementAcceleration * Time.deltaTime), movementSpeed);
        }

        private void OnDrawGizmosSelected() {
            if(_hits is null || _rayDirections is null)
                return;

            Vector3 position = transform.position;

            Gizmos.color = new Color(0.4f, 0f, 0f);
            Gizmos.DrawLine(position, (Vector2)position + _directionToTarget * _rayDistance);

            for(int i = 0; i < _rayCount; i++) {
                Color color = _canMoveInAllDirections ? Color.gray : Color.green;
                if(!_hits[i])
                    color *= 0.4f;
                color.a = 1f;

                Gizmos.color = color;
                Gizmos.DrawLine(position, (Vector2)position + _rayDirections[i] * _rayDistance);
            }

            Gizmos.color = new Color(0f, 0f, 0.4f);
            Gizmos.DrawLine(position, (Vector2)position + _targetMovement * _rayDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, (Vector2)position + _movement * _rayDistance);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_targetPosition, 0.2f);
            Gizmos.DrawWireSphere(position, _targetReachedDistance);
        }
    }
}
