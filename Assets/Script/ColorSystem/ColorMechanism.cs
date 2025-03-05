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
                    rectTransform.anchoredPosition = prevRect.anchoredPosition + new Vector2(width + 10, 0); //here's the gap
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
            } else {
                Debug.Log("null");
            }
        }
    }
}
