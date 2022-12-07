using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }
    public void Load()
    {
        SceneManager.LoadScene("Game");
    }

}
