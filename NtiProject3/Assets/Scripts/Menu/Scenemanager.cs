using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public void load(int v)
    {
        SceneManager.LoadScene(v);
    }
    public void Exit() {
        Application.Quit();
    }
}
