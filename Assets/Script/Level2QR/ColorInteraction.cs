using UnityEngine;

public class ColorInteraction : MonoBehaviour
{
    [SerializeField] private ColorMechanism colorMechanism;
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private QRCodeColorManagerL2 qrCodeColorManager;
    [SerializeField] private Collider triggerZone; // Reference to the child collider

    void Start()
    {
        // Find the ColorControl GameObject in the scene and get its ColorMechanism
        GameObject colorControl = GameObject.Find("ColorControl");
        if (colorControl != null)
        {
            colorMechanism = colorControl.GetComponent<ColorMechanism>();
        }
        else
        {
            Debug.LogError("ColorControl GameObject not found in the scene!");
        }

        // Find QRCodeColorManagerL2 (can be on the same object or elsewhere)
        qrCodeColorManager = FindObjectOfType<QRCodeColorManagerL2>();
        if (qrCodeColorManager == null)
        {
            Debug.LogError("QRCodeColorManagerL2 script not found in the scene!");
        }

        // Find the child collider named "DetectedZoneQR"
        Transform detectedZone = transform.Find("DetectedZoneQR");
        if (detectedZone != null)
        {
            triggerZone = detectedZone.GetComponent<Collider>();
            if (triggerZone != null)
            {
                triggerZone.isTrigger = true; // Ensure it's a trigger
            }
            else
            {
                Debug.LogError("No Collider found on 'DetectedZoneQR'!");
            }
        }
        else
        {
            Debug.LogError("Child object 'DetectedZoneQR' not found!");
        }
    }

    void Update()
    {
        // If the player is in range and presses "E", apply the fetched color
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (colorMechanism != null && qrCodeColorManager != null)
            {
                Debug.Log("Applying fetched color to selected color.");
                colorMechanism.ApplyFullColor(qrCodeColorManager.fetchedColor);
            }
        }
    }

    // Detect if the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered DetectedZoneQR");
        }
    }

    // Detect if the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left DetectedZoneQR");
        }
    }
}
