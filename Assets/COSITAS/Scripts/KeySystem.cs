using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeySystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration = 2f;

    private bool hasKey = false;
    private bool hasFoundDoor = false;
    private bool isNearInteractuable = false;
    private bool isNearKey = false;
    private bool isNearFinish = false;
    private Coroutine messageCoroutine;

    private void Start()
    {
        messageText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isNearFinish)
            {
                hasFoundDoor = true;
                if (!hasKey)
                    ShowMessage("Necesitas encontrar la llave de salida...");
                else
                    ShowMessage("Pulsa E para salir");
                // SceneManager.LoadScene("nombreEscena");
            }
            else if (isNearKey && hasFoundDoor)
            {
                hasKey = true;
                ShowMessage("Llave adquirida.");
                DisableAllInteractuables();
            }
            else if (isNearInteractuable)
            {
                if (hasFoundDoor)
                    ShowMessage("Aquí no hay nada...");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactuable"))
        {
            isNearInteractuable = true;
            if (hasFoundDoor)
                ShowMessage("E para buscar");
        }

        if (collision.CompareTag("Key"))
        {
            isNearKey = true;
            if (hasFoundDoor)
                ShowMessage("E para buscar");
        }

        if (collision.CompareTag("Finish"))
        {
            isNearFinish = true;
            ShowMessage("Pulsa E para salir");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactuable")) isNearInteractuable = false;
        if (other.CompareTag("Key")) isNearKey = false;
        if (other.CompareTag("Finish")) isNearFinish = false;

        messageText.gameObject.SetActive(false);
    }

    void DisableAllInteractuables()
    {
        GameObject[] interactuables = GameObject.FindGameObjectsWithTag("Interactuable");
        foreach (GameObject obj in interactuables)
            obj.SetActive(false);
    }

    void ShowMessage(string message)
    {
        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);
        messageCoroutine = StartCoroutine(MessageRoutine(message));
    }

    IEnumerator MessageRoutine(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        messageText.gameObject.SetActive(false);
    }
}
