using UnityEngine;
using TMPro;

public class DialogUIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel; // UI Panel that shows dialogue
    [SerializeField] private TextMeshProUGUI dialogueText; // Text UI for dialogue
    [SerializeField] private playerController playerController; // Reference to player movement script

    private DialoguesData currentDialogue;
    private int dialogueIndex = 0;
    private bool isDialogueActive = false;

    void Update()
    {
        // Only allow pressing "E" to progress when dialogue is active
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            MoveNextDialogue();
        }
    }

    public void StartDialogue(DialoguesData dialogueData)
    {
        if (dialogueData == null) return;

        currentDialogue = dialogueData;
        dialogueIndex = 0;
        isDialogueActive = true;

        // Freeze Player Controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        dialoguePanel.SetActive(true);
        ShowCurrentDialogue();
    }

    private void MoveNextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex >= currentDialogue.dialogueEntries.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentDialogue();
        }
    }

    private void ShowCurrentDialogue()
    {
        if (currentDialogue != null && dialogueIndex < currentDialogue.dialogueEntries.Count)
        {
            dialogueText.text = currentDialogue.dialogueEntries[dialogueIndex];
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;

        // Unfreeze Player Controls
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        dialoguePanel.SetActive(false);
    }
}
