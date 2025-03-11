using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialoguesData dialogueData; // Assign dialogue in Inspector
    [SerializeField] private DialogUIManager dialogueUI; // Reference to the dialogue system

    private bool hasTriggered = false; // Prevent multiple triggers

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // Ensure it's a player and hasn't triggered before
        {
            if (dialogueUI != null && dialogueData != null)
            {
                dialogueUI.StartDialogue(dialogueData);
                hasTriggered = true; // Mark as triggered to prevent repeats
            }
            else
            {
                Debug.LogWarning("DialogueUIManager or DialogueData is missing in DialogueTrigger!");
            }
        }
    }
}
