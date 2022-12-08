using UnityEngine;

namespace TheGameIdk.Interactive {
    public class PlayerInteractive : MonoBehaviour {
        [SerializeField] private Collider2D _interactiveZone;
        [SerializeField] private GameObject _helpPanel;

        private void OnCollisionEnter2D(Collision2D collision) {
            _helpPanel.SetActive(!_helpPanel.activeSelf);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            _helpPanel.SetActive(!_helpPanel.activeSelf);
        }
    }
}
