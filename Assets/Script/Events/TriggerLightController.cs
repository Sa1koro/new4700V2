using System.Collections;
using UnityEngine;

public class TriggerLightController : MonoBehaviour
{
    public Light targetLight; // The light source to control
    public float fadeDuration = 3f; // Duration for the light intensity change
    public float targetIntensity = 0.15f; // Final intensity of the light

    private bool hasTriggered = false; // To ensure the light changes only once

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // Check if it's the player
        {
            hasTriggered = true; // Mark as triggered
            StartCoroutine(FadeInLight()); // Start the fade-in effect
        }
    }

    private IEnumerator FadeInLight()
    {
        float initialIntensity = targetLight.intensity; // Starting intensity (should be 0 initially)
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            targetLight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / fadeDuration);
            yield return null;
        }

        targetLight.intensity = targetIntensity; // Ensure the final intensity is set
    }
}
