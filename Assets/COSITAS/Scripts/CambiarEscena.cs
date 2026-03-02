using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;


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

    [Header("Lore Panel")]
    [SerializeField] private GameObject lorePanel;
    [SerializeField] private TMP_Text[] lorePages;
    [SerializeField] private float typewriterSpeed = 0.03f;

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

        // Desactiva todos los TMP al inicio
        foreach (TMP_Text page in lorePages)
            page.gameObject.SetActive(false);
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
            StartCoroutine(ShowLoreThenTransition());
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

    private IEnumerator ShowLoreThenTransition()
    {
        isTransitioning = true;

        fadeImage.raycastTarget = true;
        float tiempo = 0f;
        while (tiempo < fadeDuration)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / fadeDuration);
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        lorePanel.SetActive(true);

        foreach (TMP_Text page in lorePages)
            page.gameObject.SetActive(false);

        foreach (TMP_Text page in lorePages)
        {
            string fullText = page.text;
            page.text = "";
            page.gameObject.SetActive(true);

            foreach (char letra in fullText)
            {
                page.text += letra;
                yield return new WaitForSeconds(typewriterSpeed);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
            page.gameObject.SetActive(false);
        }

        lorePanel.SetActive(false);
        fadeImage.raycastTarget = true;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene(nombreEscena);
    }
}
