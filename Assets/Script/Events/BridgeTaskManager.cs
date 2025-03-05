using System.Collections;
using UnityEngine;

public class SpecialEventManager : MonoBehaviour
{
    [Header("Bonded Objects")]
    public DrainableObject targetDrainableObject; // The DrainableObject to monitor
    public GameObject disableObjectA; // First object to disable
    public GameObject disableObjectB; // Second object to disable
    public GameObject enableObject; // Object to enable

    [Header("Settings")]
    public float redDistanceThreshold = 50f; // Threshold for the distance from red

    private void Update()
    {
        // Check if the target DrainableObject meets the condition
        if (targetDrainableObject != null && IsFarFromRed(targetDrainableObject.initialColor, redDistanceThreshold))
        {
            TriggerEvent();
        }
    }

    private void TriggerEvent()
    {
        // Disable the first object
        if (disableObjectA != null && disableObjectA.activeSelf)
        {
            disableObjectA.SetActive(false);
        }

        // Disable the second object
        if (disableObjectB != null && disableObjectB.activeSelf)
        {
            disableObjectB.SetActive(false);
        }

        // Enable the third object
        if (enableObject != null && !enableObject.activeSelf)
        {
            enableObject.SetActive(true);
        }
    }

    private bool IsFarFromRed(Color color, float threshold)
    {
        // Calculate the Euclidean distance from the color to pure red (255, 0, 0)
        float redDistance = Mathf.Sqrt(
            Mathf.Pow(color.r * 255 - 255, 2) +  // Red channel difference
            Mathf.Pow(color.g * 255, 2) +       // Green channel difference
            Mathf.Pow(color.b * 255, 2)         // Blue channel difference
        );
        return redDistance > threshold; // Return true if the distance exceeds the threshold
    }
}
