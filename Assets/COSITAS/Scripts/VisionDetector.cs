using UnityEngine;

public class VisionDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableHighlight interactable = other.GetComponent<InteractableHighlight>();
        if (interactable != null)
        {
            interactable.PlayerEntered();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableHighlight interactable = other.GetComponent<InteractableHighlight>();
        if (interactable != null)
        {
            interactable.PlayerExited();
        }
    }
}