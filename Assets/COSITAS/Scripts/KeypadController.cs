using UnityEngine;
using TMPro;
using System.Collections;

public class KeypadController : MonoBehaviour
{
    [Header("Slots visuales")]
    [SerializeField] private TextMeshProUGUI slot1;
    [SerializeField] private TextMeshProUGUI slot2;
    [SerializeField] private TextMeshProUGUI slot3;

    [Header("Panel")]
    [SerializeField] private GameObject keypadPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip errorSound;

    private string currentInput = "";
    private string correctCode = "123";

    private Color normalColor = Color.white;
    private Color errorColor = Color.red;
    private Color successColor = new Color(0f, 1f, 0.3f);

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void OnNumberPressed(int number)
    {
        if (currentInput.Length >= 3)
            return;

        // 🔘 Sonido botón con ligera variación
        if (audioSource != null && buttonSound != null)
        {
            float randomPitch = Random.Range(0.97f, 1.03f);
            float randomVolume = Random.Range(0.9f, 1f);

            audioSource.pitch = randomPitch;
            audioSource.PlayOneShot(buttonSound, randomVolume);
            audioSource.pitch = 1f;
        }

        currentInput += number.ToString();
        UpdateDisplay();

        if (currentInput.Length == 3)
        {
            CheckCode();
        }
    }

    void UpdateDisplay()
    {
        slot1.text = currentInput.Length > 0 ? currentInput[0].ToString() : "";
        slot2.text = currentInput.Length > 1 ? currentInput[1].ToString() : "";
        slot3.text = currentInput.Length > 2 ? currentInput[2].ToString() : "";

        slot1.color = normalColor;
        slot2.color = normalColor;
        slot3.color = normalColor;
    }

    void CheckCode()
    {
        if (currentInput == correctCode)
        {
            StartCoroutine(SuccessEffect());
        }
        else
        {
            StartCoroutine(ErrorEffect());
        }
    }

    IEnumerator SuccessEffect()
    {
        // Color verde
        slot1.color = successColor;
        slot2.color = successColor;
        slot3.color = successColor;

        // 🔊 Sonido correcto
        if (audioSource != null && successSound != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(successSound);
        }

        // 📳 Vibración suave
        yield return StartCoroutine(ShakeEffect(0.25f, 4f));

        // 🔥 Cerrar panel
        if (keypadPanel != null)
        {
            keypadPanel.SetActive(false);
        }
    }

    IEnumerator ErrorEffect()
    {
        // Color rojo
        slot1.color = errorColor;
        slot2.color = errorColor;
        slot3.color = errorColor;

        // 🔊 Sonido error
        if (audioSource != null && errorSound != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(errorSound);
        }

        // 📳 Vibración fuerte
        yield return StartCoroutine(ShakeEffect(0.4f, 10f));

        yield return new WaitForSeconds(0.2f);

        ClearInput();
    }

    IEnumerator ShakeEffect(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }
}
