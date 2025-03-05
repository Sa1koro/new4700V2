using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Bonded Objects Set 1")]
    public DrainableObject drainableObject1;
    public Light light1;
    public GameObject objectToDisable1;

    [Header("Bonded Objects Set 2")]
    public DrainableObject drainableObject2;
    public Light light2;
    public GameObject objectToDisable2;

    [Header("Bonded Objects Set 3")]
    public DrainableObject drainableObject3;
    public Light light3;
    public GameObject objectToDisable3;

    [Header("Settings")]
    public float targetLightIntensity = 20f; // Target light intensity
    public float intensityChangeDuration = 1f; // Duration for light intensity change

    private void Update()
    {
        // Check each bonded set and trigger actions
        CheckAndTrigger(drainableObject1, light1, objectToDisable1);
        CheckAndTrigger(drainableObject2, light2, objectToDisable2);
        CheckAndTrigger(drainableObject3, light3, objectToDisable3);
    }

    private void CheckAndTrigger(DrainableObject drainable, Light light, GameObject objectToDisable)
    {
        if (drainable != null && drainable.volume > 0) // Check if the volume is > 0
        {
            if (light != null)
            {
                StartCoroutine(ChangeLightIntensity(light, targetLightIntensity, intensityChangeDuration));
            }

            if (objectToDisable != null && objectToDisable.activeSelf)
            {
                objectToDisable.SetActive(false);
            }
        }
    }

    private IEnumerator ChangeLightIntensity(Light light, float targetIntensity, float duration)
    {
        float startIntensity = light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null;
        }

        light.intensity = targetIntensity; // Ensure the final intensity is set
    }
}
