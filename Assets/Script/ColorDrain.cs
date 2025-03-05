using System.Collections;
using UnityEngine;

public class ColorDrain : MonoBehaviour
{
    [Header("Sprites to Change")]
    public SpriteRenderer[] sprites; // Array of 4 SpriteRenderer objects representing the player's inventory or "bags."
    public SpriteUnit[] spriteUnits; // Array of 4 SpriteUnit components for managing the bag logic.

    [Header("Settings")]
    public float colorChangeDuration = 5f; // Duration for the gradual color change effect when draining.
    public float spriteVisibilityDuration = 10f; // Duration for which bag sprites remain visible when interacted with.

    private bool isDraining = false; // Flag to indicate if the player is currently draining.
    private int selectedBagIndex = -1; // Tracks the currently selected bag index (-1 means none selected).
    private Coroutine visibilityCoroutine; // Coroutine to manage sprite visibility timeout.

    private void Update()
    {
        HandleBagSelection();
        HandleBagReset();
        HandleBagFill(); // Add functionality to fill color to a DrainableObject.
    }

    // Method to handle selecting bags with 1/2/3/4 keys.
    private void HandleBagSelection()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Check if the corresponding key is pressed (1/2/3/4).
            {
                SelectBag(i);
                ShowBagSprites(); // Make sprites visible when a bag is selected.
                return;
            }
        }
    }

    // Method to handle resetting the selected bag with the backspace key.
    private void HandleBagReset()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && selectedBagIndex != -1) // Check if backspace is pressed and a bag is selected.
        {
            spriteUnits[selectedBagIndex].ResetUnit(); // Reset the selected bag.
        }
    }

    private void HandleBagFill()
    {
        if (Input.GetKeyDown(KeyCode.C) && selectedBagIndex != -1) // Check if D is pressed and a bag is selected.
        {
            // Perform a raycast to detect the object in front of the player.
            RaycastHit hit;
            Vector3 forward = transform.forward;
            if (Physics.Raycast(transform.position, forward, out hit, 5f)) // Adjust the raycast distance as needed.
            {
                DrainableObject drainable = hit.collider.GetComponent<DrainableObject>();
                if (drainable != null) // Ensure the object has a DrainableObject script.
                {
                    SpriteUnit selectedUnit = spriteUnits[selectedBagIndex];
                    if (selectedUnit != null && selectedUnit.unitValue > 0) // Ensure the selected bag has a color to give.
                    {
                        // Call the `fillColor` method on the DrainableObject.
                        bool success = drainable.FillColor(selectedUnit.unitColor, selectedUnit.unitValue);
                        if (success)
                        {
                            // Reset the selected bag after successfully filling color.
                            selectedUnit.ResetUnit();
                        }
                    }
                }
            }
        }
    }

    // Method to select a specific bag by index.
    private void SelectBag(int index)
    {
        if (selectedBagIndex != -1) // Deselect the previously selected bag if any.
        {
            spriteUnits[selectedBagIndex].StopFlickerAnimation();
        }

        selectedBagIndex = index; // Update the selected bag index.
        spriteUnits[selectedBagIndex].StartFlickerAnimation(); // Start flickering animation for the newly selected bag.
    }

    // Method to make all bag sprites visible for a limited time.
    private void ShowBagSprites()
    {
        if (visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine); // Stop any existing visibility timer.
        }

        foreach (var sprite in sprites)
        {
            sprite.enabled = true; // Make all sprites visible.
        }

        visibilityCoroutine = StartCoroutine(HideBagSpritesAfterDelay()); // Start the visibility timeout coroutine.
    }

    // Coroutine to hide bag sprites after a delay.
    private IEnumerator HideBagSpritesAfterDelay()
    {
        yield return new WaitForSeconds(spriteVisibilityDuration); // Wait for the defined duration.

        foreach (var sprite in sprites)
        {
            sprite.enabled = false; // Hide all sprites.
        }
    }

    // Triggered when the player is within a trigger collider.
    private void OnTriggerStay(Collider collision)
    {
        DrainableObject drainable = collision.GetComponent<DrainableObject>();

        // Handle draining with "E"
        if (drainable != null && Input.GetKey(KeyCode.E)) // Check if the collided object is drainable and the "E" key is pressed.
        {
            if (!isDraining && selectedBagIndex != -1) // Ensure a bag is selected before draining.
            {
                isDraining = true;
                drainable.StartDraining(this);

                SpriteUnit selectedUnit = spriteUnits[selectedBagIndex];
                if (selectedUnit != null && selectedUnit.unitValue < 5) // Ensure the selected bag can hold more volume.
                {
                    StartCoroutine(ChangeColorGradually(selectedUnit, drainable.initialColor, drainable));
                }
                else
                {
                    drainable.StopDraining(); // Stop draining if the selected bag is full.
                    isDraining = false;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E)) // Stop draining when the "E" key is released.
        {
            if (isDraining)
            {
                isDraining = false;
                drainable?.StopDraining();
            }
        }

        // Handle filling with "D"
        if (drainable != null && Input.GetKeyDown(KeyCode.C)) // Check if the "D" key is pressed while in range of a DrainableObject.
        {
            SpriteUnit selectedUnit = spriteUnits[selectedBagIndex];
            if (selectedUnit != null && selectedUnit.unitValue > 0) // Ensure the selected bag has a color to give.
            {
                // Call the `fillColor` method on the DrainableObject
                bool success = drainable.FillColor(selectedUnit.unitColor, selectedUnit.unitValue);
                if (success)
                {
                    // Reset the selected bag after successfully filling color
                    selectedUnit.ResetUnit();
                }
            }
        }
    }


    // Gradually changes the color of a SpriteUnit to match the targetColor, updating its value.
    private IEnumerator ChangeColorGradually(SpriteUnit spriteUnit, Color targetColor, DrainableObject drainable)
    {
        Color initialColor = spriteUnit.unitColor; // Store the initial color of the SpriteUnit.
        float elapsedTime = 0f;

        while (elapsedTime < colorChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / colorChangeDuration; // Calculate the interpolation factor.
            spriteUnit.SetUnitValue(Color.Lerp(initialColor, targetColor, t), Mathf.Clamp(spriteUnit.unitValue + 1, 0, 5)); // Update color and value.
            yield return null;
        }

        spriteUnit.SetUnitValue(targetColor, 5); // Ensure the SpriteUnit reaches the target state at the end.

        isDraining = false;
        drainable.StopDraining();
    }
}
