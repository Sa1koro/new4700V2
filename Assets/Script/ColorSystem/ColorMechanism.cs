using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorMechanism : MonoBehaviour
{
    public ColorData colorData; // Assign manually
    public GameObject colorPrefab; // Prefab to spawn
    public Vector3 offset = new Vector3(10f, 0f, 0f); // Offset for subsequent prefabs
    public Transform Canvas; // Assign your UI Canvas in Inspector

    private void Start()
    {
        if (colorData == null || colorPrefab == null || Canvas == null) return;

        GameObject previousPrefab = null; // Store the last spawned prefab

        for (int i = 0; i < colorData.elements.Count; i++)
        {
            GameObject spawnedObject = Instantiate(colorPrefab, Canvas);
            
            // If it's not the first prefab, adjust its position
            if (previousPrefab != null)
            {
                spawnedObject.transform.position = previousPrefab.transform.position + offset;
            }

            // Store this prefab as the last spawned one
            previousPrefab = spawnedObject;

            // Setup the prefab with data
            SetupPrefab(spawnedObject, colorData.elements[i]);
        }
    }

    private void SetupPrefab(GameObject spawnedObject, ColorData.ourColors element)
    {
        // Find child "ColorName" and update TextMeshPro
        Transform colorNameTransform = spawnedObject.transform.Find("ColorName");
        if (colorNameTransform != null)
        {
            TextMeshProUGUI textComponent = colorNameTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
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
                spriteRenderer.fillAmount = element.SourceAmount; // Directly setting, since it's already 0-1
            }
        }
    }
}
