using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    private bool playerInRange = false;

    [SerializeField] private GameObject interactionPanel;

    private SpriteRenderer sr;
    private Color originalColor;
    [SerializeField] private Color highlightColor = Color.cyan;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactionPanel.SetActive(true);
        }
        if (interactionPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
{
    interactionPanel.SetActive(false);
}
    }

    public void PlayerEntered()
    {
        playerInRange = true;
        sr.color = highlightColor;
    }

    public void PlayerExited()
    {
        playerInRange = false;
        sr.color = originalColor;
    }
}
