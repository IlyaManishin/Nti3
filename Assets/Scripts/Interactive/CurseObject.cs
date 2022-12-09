using UnityEngine;

namespace TheGameIdk
{
    public class CurseObject : MonoBehaviour {
        [SerializeField] private float _curseTime;
        

        private void OnTriggerEnter2D(Collider2D collision) {
            collision.gameObject.GetComponent<PlayerHealth>().setCurse(_curseTime);
        }
    }
}
