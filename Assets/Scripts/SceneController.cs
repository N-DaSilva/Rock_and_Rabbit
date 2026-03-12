using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController: MonoBehaviour
{

    public static SceneController Instance;

    private List<string> sceneHistory = new List<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string newScene)
    {
        sceneHistory.Add(SceneManager.GetActiveScene().name);
        sceneHistory.Add(newScene);
        SceneManager.LoadScene(newScene);
    }

    public void PreviousScene()
    {
        if (sceneHistory.Count >= 2)
        {
            string sceneToLoad = sceneHistory[0];
            sceneHistory.Clear();
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}