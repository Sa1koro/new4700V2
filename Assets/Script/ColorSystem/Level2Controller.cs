using UnityEngine;

public class Level2Controller : MonoBehaviour
{
    private GameObject detectedStatue; // Stores the detected statue object
    [SerializeField] private ColorMechanism colorMechanism;

    void Update()
    {
        // Check if "T" key is pressed
        if (Input.GetKeyDown(KeyCode.T) && detectedStatue != null)
        {
            // Try to get the ColorUpdater script
            ColorUpdater colorUpdater = detectedStatue.GetComponentInParent<ColorUpdater>();

            if (colorUpdater != null)
            {
                colorUpdater.TryTeleport(); // Call TryTeleport() if found
                Debug.Log("TryTeleport");
            } else {
                Debug.Log("didn't find colorscript");
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && detectedStatue != null)
        {
            colorMechanism.ChangingStatuesColor(detectedStatue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the "statues" tag
        if (other.CompareTag("Statues"))
        {
            detectedStatue = other.gameObject; // Store reference
            Debug.Log("detected statue: " + other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove reference when leaving trigger zone
        if (other.gameObject == detectedStatue)
        {
            detectedStatue = null;
        }
    }
}
