using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour
{
    public GameObject screaamerPanel;  // Panel con la imagen animada
    public float duration = 1.5f;
    public bool isShowing = false;

    void Start()
    {
        if (isShowing) return;
        screaamerPanel.SetActive(false);
    }

    public void ShowScreamer()
    {
        StartCoroutine(ScreamerRoutine());
    }

    IEnumerator ScreamerRoutine()
    {
        isShowing = true;
        screaamerPanel.SetActive(true);
        Time.timeScale = 0f;  // ? pausa el juego

        // WaitForSecondsRealtime porque timeScale es 0
        yield return new WaitForSecondsRealtime(duration);

        screaamerPanel.SetActive(false);
        Time.timeScale = 1f;  // ? reanuda
        isShowing = false;
    }
}
