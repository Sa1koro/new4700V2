using System.Collections;
using UnityEngine;

public class DelayedStart : MonoBehaviour
{
    [SerializeField] private Vector4 newColor = new Vector4(1, 0, 0, 1); // Default: Red (RGBA)
    [SerializeField] private float delayTime = 1f; // Delay in seconds

    private ColorUpdater colorUpdater;

    private void Awake()
    {
        colorUpdater = GetComponent<ColorUpdater>(); // Get the ColorUpdater script
        if (colorUpdater == null)
        {
            Debug.LogError("ColorUpdater script not found on " + gameObject.name);
            return;
        }

        StartCoroutine(DelayedSetColor());
    }

    private IEnumerator DelayedSetColor()
    {
        yield return new WaitForSeconds(delayTime); // Wait for the specified delay
        colorUpdater.SetColor(newColor); // Call SetColor on ColorUpdater
        Debug.Log(gameObject.name + " changed color to " + newColor + " after " + delayTime + " seconds.");
    }
}
