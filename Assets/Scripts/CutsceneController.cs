using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class CutsceneController : MonoBehaviour
{

    private Image panel;
    [SerializeField] private VideoPlayer VideoPlayer;
    [SerializeField] private GameObject bgMusic;
     private AudioSource audioSource;

    void Start()
    {
        panel = GameObject.FindWithTag("Panel").GetComponent<Image>();
        audioSource = bgMusic.GetComponent<AudioSource>();
        VideoPlayer.loopPointReached += LoadScene;

        StartCoroutine(FadePanelOut(0.2f));
    }

    private IEnumerator FadePanelIn(float duration)
    {
        yield return new WaitForSeconds(3);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panel.color = Color.Lerp(new Color(0, 0, 0, 0f), new Color(0, 0, 0, 1f), elapsedTime / duration);
            yield return null;
        }

        panel.color = new Color(0, 0, 0, 1f);
        SceneManager.LoadScene("Level1");
    }

    private IEnumerator FadePanelOut(float duration)
    {
        audioSource.volume = Mathf.Lerp(0.5f, 0f, Time.time);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panel.color = Color.Lerp(new Color(0, 0, 0, 1f), new Color(0, 0, 0, 0f), elapsedTime / duration);
            yield return null;
        }

        panel.color = new Color(0, 0, 0, 0f);
    }

    void LoadScene(VideoPlayer vp)
    {
        StartCoroutine(FadePanelIn(1f));
    }
}
