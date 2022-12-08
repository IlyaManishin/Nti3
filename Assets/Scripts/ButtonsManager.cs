using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheGameIdk {
    public class ButtonsManager : MonoBehaviour {
        public void Load(string name = "Game") => SceneManager.LoadScene(name);
        public void Exit() => Application.Quit();
    }
}
