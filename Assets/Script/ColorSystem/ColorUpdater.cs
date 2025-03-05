using UnityEngine;
using UnityEngine.UI;  // For UI support, if needed

public class ColorUpdater : MonoBehaviour
{
    // Serialized private field for the color value (RGBA).
    // RGB control the emission’s color, and A controls the emission alpha as described.
    [SerializeField]
    private Vector4 colorValue = new Vector4(1f, 1f, 1f, 1f);
    
    // Stored default value so we can reset.
    [SerializeField]
    private Vector4 defaultColor = new Vector4(1f, 1f, 1f, 1f);

    // This will hold our copied material (the one on UpperPart) so changes affect only this object.
    private Material upperPartMaterial;

    void Awake()
    {
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
}
