using UnityEngine;

public class UIController : MonoBehaviour
{
    private SceneController sceneController;

    void Start ()
    {
        sceneController = GameObject.FindWithTag("SceneHandler").GetComponent<SceneController>();
    }

    public void Retry()
    {
        sceneController.PreviousScene();
    }
}
