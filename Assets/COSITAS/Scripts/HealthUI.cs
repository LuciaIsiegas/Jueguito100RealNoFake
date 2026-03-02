using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public int maxHealth = 3;
    private int currentHealth;
    public Screamer screamerUI;
    public GameObject heartsPanel;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int amount)
    {
        if (screamerUI.isShowing) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
        StartCoroutine(HideHeartsWhileScreamer());
    }

    IEnumerator HideHeartsWhileScreamer()
    {
        heartsPanel.SetActive(false);
        screamerUI.ShowScreamer();
        yield return new WaitForSeconds(screamerUI.duration);
        heartsPanel.SetActive(true);
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth ? true : false;

        }
    }
}
