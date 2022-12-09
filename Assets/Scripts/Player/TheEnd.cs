using UnityEngine;
using UnityEngine.UIElements;

namespace TheGameIdk
{
    public class TheEnd : MonoBehaviour
    {
        [SerializeField] private GameObject _endPanel;

        public void TheEndding() {
            _endPanel.SetActive(true);
        } 
    }
}
