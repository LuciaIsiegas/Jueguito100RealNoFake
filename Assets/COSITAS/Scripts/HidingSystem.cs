using System.Collections;
using UnityEngine;

public class HidingSystem : MonoBehaviour
{
    [Header("Configuración")]
    public float hideDuration = 5f;
    public KeyCode hideKey = KeyCode.E;

    [Header("Referencias")]
    public Renderer[] characterRenderers;
    public GameObject uiHelpText;

    private Collider2D characterCollider;
    private bool isNearHidingPoint = false;
    private bool isHiding = false;
    private Coroutine hideCoroutine;

    public bool IsHiding => isHiding;

    private void Start()
    {
        characterCollider = GetComponent<Collider2D>();

        if (characterRenderers == null || characterRenderers.Length == 0)
            characterRenderers = GetComponentsInChildren<Renderer>();

        if (uiHelpText != null)
            uiHelpText.SetActive(false);
    }

    void Update()
    {
        if (isNearHidingPoint && !isHiding)
        {
            if (Input.GetKeyDown(hideKey) || Input.GetMouseButtonDown(0))
            {
                StartHiding();
            }
        }


        // Salir del escondite manualmente presionando E
        if (isHiding && Input.GetKeyDown(hideKey))
        {
            StopHiding();
        }
    }

    void StartHiding()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideRoutine());
    }

    IEnumerator HideRoutine()
    {
        isHiding = true;
        SetVisible(false);
        if (uiHelpText != null) uiHelpText.SetActive(false);
        Debug.Log("¡Personaje escondido!");

        yield return new WaitForSeconds(hideDuration);

        isHiding = false;
        SetVisible(true);
        Debug.Log("Personaje visible de nuevo.");
    }

    void SetVisible(bool visible)
    {
        foreach (Renderer r in characterRenderers)
            r.enabled = visible;

        if (characterCollider != null)
            characterCollider.enabled = visible;
    }

    // *** 2D ***
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("Hiding"))
        {
            isNearHidingPoint = true;
            if (uiHelpText != null) uiHelpText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hiding"))
        {
            isNearHidingPoint = false;
            if (uiHelpText != null) uiHelpText.SetActive(false);
        }
    }
    void StopHiding()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        isHiding = false;
        SetVisible(true);
        if (uiHelpText != null && isNearHidingPoint)
            uiHelpText.SetActive(true);
        Debug.Log("Personaje salió del escondite.");
    }
}