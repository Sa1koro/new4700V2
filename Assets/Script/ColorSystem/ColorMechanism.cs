using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ColorMechanism : MonoBehaviour
{
    [SerializeField] private ColorData colorData; // Assign manually
    [SerializeField] private GameObject colorPrefab; // Prefab to spawn
    [SerializeField] private Transform Canvas; // Assign UI Canvas in Inspector

    private Dictionary<int, GameObject> spawnedPrefabs = new Dictionary<int, GameObject>();
    private int lastElementCount = 0; // Track the last known count of elements

    public int selectedIndex = -1; // Store selected element index (-1 means none selected)

    private void Update()
    {
        if (colorData == null || colorPrefab == null || Canvas == null) return;

        // If number of elements changes, refresh everything
        if (colorData.elements.Count != lastElementCount)
        {
            RefreshElements();
            lastElementCount = colorData.elements.Count;
        }
        else
        {
            // Just update existing prefabs
            for (int i = 0; i < colorData.elements.Count; i++)
            {
                if (spawnedPrefabs.TryGetValue(i, out GameObject prefab))
                {
                    UpdatePrefab(prefab, colorData.elements[i]);
                }
            }
        }

        // Handle input for selecting colors
        HandleSelectionInput();
    }

    private void RefreshElements()
    {
        // Remove all current prefabs before reloading
        foreach (var prefab in spawnedPrefabs.Values)
        {
            Destroy(prefab);
        }
        spawnedPrefabs.Clear();

        for (int i = 0; i < colorData.elements.Count; i++)
        {
            var element = colorData.elements[i];

            // Spawn and parent the object to Canvas
            GameObject spawnedObject = Instantiate(colorPrefab, Canvas);
            spawnedPrefabs[i] = spawnedObject;

            // Get the RectTransform
            RectTransform rectTransform = spawnedObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Move every element (except the first one) to the right of the previous one
                if (i > 0)
                {
                    RectTransform prevRect = spawnedPrefabs[i - 1].GetComponent<RectTransform>();
                    float width = prevRect.rect.width * prevRect.localScale.x;
                    rectTransform.anchoredPosition = prevRect.anchoredPosition + new Vector2(width + 10, 0); // Spacing
                }
            }

            UpdatePrefab(spawnedObject, element);
        }
    }

    private void UpdatePrefab(GameObject spawnedObject, ColorData.ourColors element)
    {
        // Find child "ColorName" and update TextMeshPro text
        Transform colorNameTransform = spawnedObject.transform.Find("ColorName");
        if (colorNameTransform != null)
        {
            TextMeshProUGUI textComponent = colorNameTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null && textComponent.text != element.ColorName)
            {
                textComponent.text = element.ColorName;
            }
        }

        // Find child "InColor" and update color & fill amount
        Transform inColorTransform = spawnedObject.transform.Find("InColor");
        if (inColorTransform != null)
        {
            Image spriteRenderer = inColorTransform.GetComponent<Image>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(element.color.x, element.color.y, element.color.z, element.color.w);
                spriteRenderer.fillAmount = element.SourceAmount;
            }
        }

        // Enable or disable "selected" child based on the element's selected state
        Transform selectedTransform = spawnedObject.transform.Find("selected");
        if (selectedTransform != null)
        {
            selectedTransform.gameObject.SetActive(element.selected);
        }
    }

    private void HandleSelectionInput()
    {
        if (colorData.elements.Count < 4) return; // Ensure at least 4 elements exist

        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + (i - 1))) // Detect key presses (1,2,3,4)
            {
                SelectColor(i - 1);
                break;
            }
        }
    }

    private void SelectColor(int index)
    {
        if (index < 0 || index >= colorData.elements.Count) return;

        // Update selected state in the elements list
        for (int i = 0; i < colorData.elements.Count; i++)
        {
            colorData.elements[i].selected = (i == index); // Only one selected at a time
        }

        selectedIndex = index; // Store the selected index
    }

    public void ChangingStatuesColor(GameObject targetObject)
    {
        // Ensure there's a valid selection
        if (selectedIndex < 0 || selectedIndex >= colorData.elements.Count)
        {
            Debug.Log("No color selected.");
            return;
        }

        // Get the selected color element
        ColorData.ourColors selectedColor = colorData.elements[selectedIndex];

        // Check if SourceAmount is enough to reduce
        if (selectedColor.SourceAmount < 0.5f)
        {
            Debug.Log("Not enough color source to apply.");
            return;
        }

        // Try to get the ColorUpdater script
        ColorUpdater colorUpdater = targetObject.GetComponentInParent<ColorUpdater>();
        if (colorUpdater != null)
        {
            // Apply the color to the target object's SetColor method
            colorUpdater.SetColor(selectedColor.color);

            // Reduce the source amount
            selectedColor.SourceAmount -= 0.5f;
        }
        else
        {
            Debug.Log("Target object does not have a ColorUpdater script.");
        }
    }

    public void ApplyFullColor(Vector4 newColor)
    {
        // Ensure a valid selection exists
        if (selectedIndex < 0 || selectedIndex >= colorData.elements.Count)
        {
            Debug.LogError("No color selected or invalid index.");
            return;
        }

        // Get the selected color element
        ColorData.ourColors selectedColor = colorData.elements[selectedIndex];

        // Apply the new color (use only RGB, keep original Alpha)
        selectedColor.color = new Vector4(newColor.x, newColor.y, newColor.z, selectedColor.color.w);

        // Set SourceAmount to full
        selectedColor.SourceAmount = 1f;

        // Update the UI display
        if (spawnedPrefabs.TryGetValue(selectedIndex, out GameObject prefab))
        {
            UpdatePrefab(prefab, selectedColor);
        }

        Debug.Log($"Updated selected color to: {selectedColor.color} with full source.");
    }
}
