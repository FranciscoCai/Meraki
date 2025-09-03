using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;         // Imagen blanca en Canvas
    public float fadeDuration = 1f; // Duración del fade
    public string sceneToLoad; // Nombre de la escena a cargar

    private bool isFading = false;

    private void Start()
    {
        // Asegura que el fade empieza transparente
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        // Detecta cualquier entrada (teclado, ratón, mando)
        if (!isFading && Input.anyKeyDown)
        {
            StartCoroutine(FadeAndLoad(sceneToLoad));
        }
    }

    // También puedes llamar esto manualmente desde un botón UI si quieres
    public void LoadScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isFading = true;

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