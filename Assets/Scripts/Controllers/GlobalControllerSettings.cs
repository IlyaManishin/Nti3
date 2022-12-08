using UnityEngine;

namespace TheGameIdk.Controllers {
    [CreateAssetMenu(fileName = "ControllerSettings", menuName = "Controller/Settings", order = 0)]
    public class GlobalControllerSettings : ScriptableObject {
        public LayerMask wallLayerMask;
    }
}
