using JetBrains.Annotations;

using UnityEngine;

namespace TheGameIdk {
    public static class Util {
        public static bool ContainsLayer(this LayerMask mask, int layer) => (mask & (1 << layer)) != 0;

        public static Vector2 DegreesToVector(float degrees) {
            float radians = degrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        // essentially set velocity direction to force direction and
        // limit the force speed so that it doesn't make the body go faster than max speed
        // but outside forces can still make it go faster than max speed, in which case
        // no speed will be added and you'll only be able to control the direction until you slow down
        public static void AddEntityForce([NotNull] this Rigidbody2D rigidbody, Vector2 force, float maxSpeed) {
            Vector2 velocity = rigidbody.velocity;
            float currentSpeed = velocity.magnitude;
            float spareSpeed = maxSpeed - currentSpeed;
            float forceSpeed = Mathf.Max(spareSpeed - force.magnitude, 0f);
            float newSpeed = velocity.magnitude + Mathf.Min(forceSpeed, maxSpeed);
            rigidbody.velocity = force.normalized * newSpeed;
        }
    }
}
