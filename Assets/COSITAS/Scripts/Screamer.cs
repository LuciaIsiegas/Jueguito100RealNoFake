using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamer : MonoBehaviour
{
    public GameObject screamerPanel;
    public float duration = 1.5f;
    public bool isShowing = false;

    [Header("Audio")]
    [SerializeField] private AudioSource musicaFondo;
    [SerializeField] private AudioSource screaamerSound;
    [SerializeField] private AudioClip screaamerClip;

    void Start()
    {
        if (isShowing) return;
        screamerPanel.SetActive(false);
    }

    public void ShowScreamer()
    {
        StartCoroutine(ScreamerRoutine());
    }

    IEnumerator ScreamerRoutine()
    {
        isShowing = true;
        screamerPanel.SetActive(true);
        Time.timeScale = 0f;

        // Para la música y reproduce el screamer
        if (musicaFondo != null) musicaFondo.Pause();
        if (screaamerSound != null && screaamerClip != null)
        {
            screaamerSound.PlayOneShot(screaamerClip);
        }

        yield return new WaitForSecondsRealtime(duration);

        screamerPanel.SetActive(false);
        Time.timeScale = 1f;

        // Reanuda la música
        if (musicaFondo != null) musicaFondo.UnPause();

        isShowing = false;
    }
}
