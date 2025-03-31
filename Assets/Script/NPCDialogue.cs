using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NPCDialogue : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI; // UI should be unique per NPC
    [SerializeField] private TextMeshProUGUI dialogueText; // Ensure each NPC has a separate text field
    [SerializeField] private Button nextButton; // Ensure each NPC has a separate button
    [SerializeField] private GameObject interactIndicator; // Unique E key prompt per NPC


    [Header("Dialogue Settings")]
    public string[] dialogueLines; // 存储对话内容
    private int currentLine = 0; // 当前对话索引

    private bool isPlayerNear = false;
    private bool isDialogueActive = false;

    private void Start()
    {
        dialogueUI.SetActive(false);
        interactIndicator.SetActive(false);

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners(); // Ensure no previous bindings exist
            nextButton.onClick.AddListener(() => AdvanceDialogue()); // Assign this NPC's button
        }
    }



    private void Update()
    {
        if (isPlayerNear && !isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        interactIndicator.SetActive(false);
        dialogueUI.SetActive(true);
        currentLine = 0;
        
        if (dialogueLines.Length > 0) 
        {
            dialogueText.text = dialogueLines[currentLine];
        }
    }

    public void AdvanceDialogue()
    {
        if (currentLine < dialogueLines.Length - 1)
        {
            currentLine++;
            dialogueText.text = dialogueLines[currentLine]; // Only update THIS NPC's dialogue
        }
        else
        {
            EndDialogue();
        }
    }


    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false); // 关闭UI
        interactIndicator.SetActive(true); // 重新显示E指示
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactIndicator.SetActive(true); // 显示E指示
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactIndicator.SetActive(false); // 关闭E指示
        }
    }
}
