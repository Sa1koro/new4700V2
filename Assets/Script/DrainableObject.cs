using System.Collections;
using UnityEngine;

public class DrainableObject : MonoBehaviour
{
    [Header("Drain Settings")]
    public Color initialColor = Color.white; // Initial color of the object, editable in the inspector.
    public int volume = 5; // Current volume of the object, editable in the inspector, max is 5.

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer to update color visually.
    private Coroutine drainCoroutine; // Reference to the active draining coroutine.
    private bool isDraining = false; // Flag to indicate whether the object is currently being drained.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component on this object.
        UpdateSpriteColor(); // Initialize the sprite's color based on `initialColor` and `volume`.
    }

    // Method to start draining this object when triggered by the player.
    public void StartDraining(ColorDrain colorDrainScript)
    {
        if (!isDraining && volume > 0) // Ensure the object is not already being drained and has volume left.
        {
            isDraining = true;
            if (drainCoroutine != null)
            {
                StopCoroutine(drainCoroutine); // Stop any active draining coroutine.
            }
            drainCoroutine = StartCoroutine(DrainVolume()); // Start the volume draining process.
        }
    }

    // Method to stop draining this object.
    public void StopDraining()
    {
        if (isDraining)
        {
            isDraining = false; // Reset the draining flag.
            if (drainCoroutine != null)
            {
                StopCoroutine(drainCoroutine); // Stop the active draining coroutine.
            }
        }
    }

    // Coroutine to gradually reduce volume and update color in real-time.
    private IEnumerator DrainVolume()
    {
        while (volume > 0 && isDraining)
        {
            volume -= 1; // Reduce volume by 1.
            UpdateSpriteColor(); // Update the sprite's color based on the new volume.

            if (volume <= 0) // If volume is depleted, reset the initial color to white.
            {
                initialColor = Color.white;
                UpdateSpriteColor(); // Ensure the sprite reflects the reset color.
                StopDraining(); // Stop the draining process.
                yield break; // Exit the coroutine.
            }

            yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before reducing volume again.
        }
    }

    // Updates the sprite's color based on the current `initialColor` and `volume`.
    private void UpdateSpriteColor()
    {
        if (spriteRenderer != null)
        {
            float intensity = volume / 5f; // Calculate intensity as a fraction of the max volume.
            spriteRenderer.color = Color.Lerp(Color.white, initialColor, intensity); // Blend between white and the initial color based on intensity.
        }
    }

    public bool FillColor(Color color, int volumeToAdd)
    {
        if (volume > 0) return false; // Only allow filling if the object is currently white (volume == 0).

        if (IsColorCompatible(color)) // Check if the incoming color is compatible with white.
        {
            initialColor = color; // Update the initial color.
            volume = Mathf.Clamp(volume + volumeToAdd, 0, 5); // Add the volume and clamp it between 0 and 5.
            UpdateSpriteColor(); // Update the sprite's color based on the new volume.
            return true; // Return true if the color was successfully filled.
        }

        return false; // Return false if the color is not compatible.
    }
    
    private bool IsColorCompatible(Color color)
    {
        return initialColor == Color.white; // Only compatible if the object's current color is white.
    }


}
