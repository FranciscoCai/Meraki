using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string sceneToLoad;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip fadeSound;

    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // Asigna aquí el VideoPlayer
    public GameObject videoPlayerGO; // GameObject que contiene el VideoPlayer

    private bool isFading = false;
    private bool videoFinished = false;

    private void Start()
    {
        // Configurar el fade inicial
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        // Si hay un VideoPlayer, escuchar cuando termine
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            // Si no hay video, permitir cambiar de escena directamente
            videoFinished = true;
        }
    }

    private void Update()
    {
        // Solo permitir cambio de escena cuando el video haya terminado
        if (videoFinished && !isFading && Input.anyKeyDown)
        {
            StartCoroutine(FadeAndLoad(sceneToLoad));
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoFinished = true;

        // Desactivar el GameObject que reproduce el video
        if (videoPlayerGO != null)
        {
            videoPlayerGO.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (videoFinished && !isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

        if (audioSource != null && fadeSound != null)
        {
            audioSource.PlayOneShot(fadeSound);
        }

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}