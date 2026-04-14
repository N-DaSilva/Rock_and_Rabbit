using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private SceneController sceneController;

    void Start ()
    {
        if (SceneManager.GetActiveScene().name != "MenuScreen")
        {
            sceneController = GameObject.FindWithTag("SceneHandler").GetComponent<SceneController>();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    public void Retry()
    {
        if (sceneController)
        {
            sceneController.PreviousScene();
        }
    }
}
