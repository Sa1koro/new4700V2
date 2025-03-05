using System.Collections;
using UnityEngine;

public class SpriteUnit : MonoBehaviour
{
    [Header("Bag Properties")]
    public Color unitColor = Color.white; // Current color of the sprite.
    public int unitValue = 0; // Current volume of the sprite, maximum is 5.

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component.
    private bool isSelected = false; // Tracks if this unit is currently selected (used for flickering animation).

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component.
        UpdateColor(); // Initialize the sprite color based on `unitColor` and `unitValue`.
    }

    // Method to set the color and volume of this unit.
    public void SetUnitValue(Color color, int value)
    {
        unitColor = color; // Update the color.
        unitValue = Mathf.Clamp(value, 0, 5); // Clamp the volume between 0 and 5.
        UpdateColor(); // Update the visual appearance of the sprite.
    }

    // Method to drain color into this unit. Only works if selected and the color is compatible.
    public bool DrainColor(Color color, int volumeToAdd)
    {
        if (unitValue == 0)
        {
            unitColor = Color.white; // Reset the color to white if the unit is empty.
        }

        // Check if the unit is selected and the color is compatible.
        if (isSelected && IsColorCompatible(color))
        {
            unitColor = (unitColor == Color.white) ? color : unitColor; // Set the color if it was white.
            unitValue = Mathf.Clamp(unitValue + volumeToAdd, 0, 5); // Add volume, clamped between 0 and 5.
            UpdateColor(); // Update the sprite's appearance.
            return true; // Drain successful.
        }

        return false; // Drain failed.
    }

    // Method to reset the unit's color to white and volume to 0.
    public void ResetUnit()
    {
        if (isSelected) // Check if the unit is selected.
        {
            unitColor = Color.white; // Reset color to white.
            unitValue = 0; // Reset volume to 0.
            UpdateColor(); // Update the sprite's appearance.
        }
    }

    // Coroutine to handle smooth flickering animation when the unit is selected.
    public void StartFlickerAnimation()
    {
        if (!isSelected)
        {
            isSelected = true; // Mark this unit as selected.
            StartCoroutine(FlickerAnimation()); // Start the flickering animation coroutine.
        }
    }

    // Method to stop the flickering animation and reset the state.
    public void StopFlickerAnimation()
    {
        isSelected = false; // Mark this unit as not selected.
        StopAllCoroutines(); // Stop any ongoing coroutines.
        UpdateColor(); // Restore the color without flickering.
    }

    // Coroutine for flickering animation when selected.
    private IEnumerator FlickerAnimation()
    {
        float flickerDuration = 0.5f; // Total duration of the flickering cycle.
        float elapsedTime = 0f;

        while (isSelected)
        {
            elapsedTime += Time.deltaTime;
            float intensity = Mathf.Abs(Mathf.Sin(elapsedTime * Mathf.PI / flickerDuration)); // Smooth flicker effect.
            spriteRenderer.color = Color.Lerp(Color.white, unitColor, intensity); // Interpolate between white and the current color.
            yield return null; // Wait until the next frame.
        }

        UpdateColor(); // Ensure the color is restored when flickering stops.
    }

    // Helper method to check if the new color is compatible with the unit's current color.
    private bool IsColorCompatible(Color color)
    {
        if (unitColor == Color.white) return true; // Accept any color if the current color is white.

        float distance = CalculateColorDistance(unitColor, color); // Calculate RGB distance between the colors.
        return distance <= 50; // Return true if the distance is within the threshold.
    }

    // Calculates the Euclidean distance between two colors in RGB format.
    private float CalculateColorDistance(Color color1, Color color2)
    {
        float rDiff = (color1.r - color2.r) * 255; // Difference in red channels (scaled to 255).
        float gDiff = (color1.g - color2.g) * 255; // Difference in green channels (scaled to 255).
        float bDiff = (color1.b - color2.b) * 255; // Difference in blue channels (scaled to 255).

        return Mathf.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff); // Return the Euclidean distance.
    }

    // Updates the sprite's color based on the current `unitColor` and `unitValue`.
    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            float intensity = unitValue / 5f; // Calculate intensity as a fraction of the max volume.
            spriteRenderer.color = Color.Lerp(Color.white, unitColor, intensity); // Blend between white and the current color based on intensity.
        }
    }
}
