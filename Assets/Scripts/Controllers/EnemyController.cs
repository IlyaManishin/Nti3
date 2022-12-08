using UnityEngine;

namespace TheGameIdk.Controllers {
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : AiController {
        protected override Rigidbody2D target => PlayerController.instance;
    }
}
