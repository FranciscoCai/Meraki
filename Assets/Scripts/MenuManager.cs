using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    private bool isFading = false;

    private void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if (!isFading && Input.anyKeyDown)
        {
            StartCoroutine(FadeAndLoad(sceneToLoad));
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!isFading)
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