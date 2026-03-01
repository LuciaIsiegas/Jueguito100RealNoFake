using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

#if UNITY_EDITOR
    [Header("Scene (Assign from Editor)")]
    [SerializeField] private SceneAsset escena;
#endif

    [SerializeField] private string nombreEscena;

    private bool isTransitioning = false;

    private void Awake()
    {
        // Asegura que el panel empiece negro
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 1f);
            fadeImage.raycastTarget = true;
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (escena != null)
        {
            nombreEscena = escena.name;
        }
    }
#endif

    public void CambiarEscena()
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeOutAndLoad());
        }
    }

    private IEnumerator FadeIn()
    {
        float tiempo = fadeDuration;

        while (tiempo > 0f)
        {
            tiempo -= Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / fadeDuration);

            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);

            yield return null;
        }

        fadeImage.raycastTarget = false; // Permitir clicks
    }

    private IEnumerator FadeOutAndLoad()
    {
        isTransitioning = true;
        fadeImage.raycastTarget = true; // Bloquear clicks

        float tiempo = 0f;

        while (tiempo < fadeDuration)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / fadeDuration);

            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);

            yield return null;
        }

        SceneManager.LoadScene(nombreEscena);
    }
}
