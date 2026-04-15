using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    private SceneController sceneController;

    private Image panel;

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private GameObject bgMusic;

    private AudioSource audioSource;

    private IEnumerator FadePanelIn(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panel.color = Color.Lerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f), elapsedTime / duration);
            yield return null;
        }

        panel.color = new Color(0, 0, 0, 1f);
    }

    private IEnumerator WaitAndSwitchScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Cutscene");
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MenuScreen")
        {
            sceneController = GameObject.FindWithTag("SceneHandler").GetComponent<SceneController>();
        }

        panel = GameObject.FindWithTag("Panel").GetComponent<Image>();
        audioSource = bgMusic.GetComponent<AudioSource>();
    }

    public void Hover()
    {
        AudioSource.PlayClipAtPoint(hoverSound, transform.position);
    }

    public void Play()
    {
        StartCoroutine(WaitAndSwitchScene());
        StartCoroutine(FadePanelIn(2f));
        AudioSource.PlayClipAtPoint(gameStartSound, transform.position);
        audioSource.volume = Mathf.Lerp(0.5f, 0f, Time.time);
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
