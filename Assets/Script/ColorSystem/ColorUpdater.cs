using UnityEngine;
using UnityEngine.UI;  // For UI support, if needed
using TMPro;

public class ColorUpdater : MonoBehaviour
{
    // Serialized private field for the color value (RGBA).
    // RGB control the emission’s color, and A controls the emission alpha as described.
    [SerializeField] private Vector4 colorValue = new Vector4(1f, 1f, 1f, 1f);
    
    // Stored default value so we can reset.
    [SerializeField] private Vector4 defaultColor = new Vector4(1f, 1f, 1f, 1f);
    [SerializeField] private Level2ColorDataStore Level2ColorData; // Reference to the ColorData asset


    // This will hold our copied material (the one on UpperPart) so changes affect only this object.
    [SerializeField] private Material upperPartMaterial;
    [SerializeField] private TextMeshProUGUI colorNameText; // Reference to ColorName text
    [SerializeField] private float colorThreshold = 0.3f; // If distance is above this, consider colors "too different"

    void Awake()
    {
        // Find the "ColorName" child and get its TextMeshPro component
        Transform colorNameTransform = transform.Find("Canvas/ColorName");
        if (colorNameTransform != null)
        {
            colorNameText = colorNameTransform.GetComponent<TextMeshProUGUI>();
        } else {
            Debug.Log("no text");
        }

        // Find the child named "UpperPart"
        Transform upperPartTransform = transform.Find("UpperPart");
        if (upperPartTransform != null)
        {
            // First, try to get a Renderer (for 3D objects)
            Renderer rend = upperPartTransform.GetComponent<Renderer>();
            if (rend != null)
            {
                // Create a new material from the existing one and assign it
                upperPartMaterial = new Material(rend.material);
                rend.material = upperPartMaterial;
            }
            else
            {
                // If no Renderer found, try an Image component (for UI elements)
                Image img = upperPartTransform.GetComponent<Image>();
                if (img != null)
                {
                    upperPartMaterial = new Material(img.material);
                    img.material = upperPartMaterial;
                }
            }
        }
    }

    void Update()
    {
        if (upperPartMaterial != null)
        {
            // Remap colorValue.w (0 to 1) -> (-10 to 0)
            float emissionIntensity = Mathf.Lerp(-10f, -3f, colorValue.w);

            // Create the emission color using RGB from colorValue, alpha is always 1
            Color baseEmissionColor = new Color(colorValue.x, colorValue.y, colorValue.z, 1f);

            // Apply intensity by multiplying the emission color
            Color finalEmissionColor = baseEmissionColor * Mathf.Pow(2, emissionIntensity);

            // Update the material's emission color
            upperPartMaterial.SetColor("_EmissionColor", finalEmissionColor);
            upperPartMaterial.EnableKeyword("_EMISSION");
        }

        if (Level2ColorData != null && colorNameText != null)
        {
            // Find the closest matching color from ColorData list
            string closestColorName = FindClosestColorName();
            colorNameText.text = closestColorName; // Update UI text
        }
    }

    // Public method to set the color value to a new value.
    public void SetColor(Vector4 newColor)
    {
        colorValue = newColor;
    }

    // Public method to reset the color value to the default.
    public void ResetColor()
    {
        colorValue = defaultColor;
    }

    public Vector4 GetColor()
    {
        return colorValue;
    }
    private string FindClosestColorName()
    {
        string bestMatch = "Closed"; // Default to "Closed"
        float closestDistance = float.MaxValue;

        foreach (var element in Level2ColorData.elements)
        {
            // Compare only the first three values (RGB), ignore Alpha
            Vector3 storedRGB = new Vector3(element.color.x, element.color.y, element.color.z);
            Vector3 currentRGB = new Vector3(colorValue.x, colorValue.y, colorValue.z);

            float distance = Vector3.Distance(storedRGB, currentRGB); // Calculate color similarity

            if (distance < closestDistance) // Check if this is the closest match
            {
                closestDistance = distance;
                bestMatch = element.ColorName;
            }
        }

        // If the closest color is still too different, set to "Closed"
        if (closestDistance > colorThreshold)
        {
            bestMatch = "Closed";
        }

        // Only count valid color names (skip "Closed" and unrelated names)
        if (bestMatch != "Closed")
        {
            int nameCount = 0;
            Transform parent = transform.parent; // Get parent or group
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (child.gameObject == this.gameObject) continue; // Exclude itself

                    string childName = child.gameObject.name;

                    // Check if child name exists in Level2ColorData
                    bool isValidName = Level2ColorData.elements.Exists(e => e.ColorName == childName);

                    if (isValidName && childName == bestMatch)
                    {
                        nameCount++;
                    }
                }
            }

            // If more than 2 exist (meaning the third one is trying to change to this name)
            if (nameCount >= 2)
            {
                bestMatch += " color exceeding limit";

                // Update the UI text (if available)
                Transform colorNameTransform = transform.Find("ColorName");
                if (colorNameTransform != null)
                {
                    TextMeshProUGUI textComponent = colorNameTransform.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = bestMatch;
                    }
                }
            }
        }

        // Change the GameObject name AFTER counting, to avoid double-counting issue
        gameObject.name = bestMatch;

        return bestMatch;
    }

    public void TryTeleport()
    {
        // Check if the object's name is a valid color in Level2ColorData
        bool isValidName = Level2ColorData.elements.Exists(e => e.ColorName == gameObject.name);
        if (!isValidName) return; // Exit if name is invalid

        Transform parent = transform.parent; // Get parent or group
        if (parent == null) return;

        // Find another object with the same name in the group (excluding itself)
        Transform targetTeleport = null;
        foreach (Transform child in parent)
        {
            if (child.gameObject == this.gameObject) continue; // Skip self
            if (child.gameObject.name == gameObject.name) // Match found
            {
                Transform teleportTransform = child.Find("Teleport");
                if (teleportTransform != null)
                {
                    targetTeleport = teleportTransform;
                    break; // Stop after finding the first valid one
                }
            }
        }

        // If a valid teleport target was found, move the player
        if (targetTeleport != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetTeleport.position; // Move player
            }
        }
    }
}
