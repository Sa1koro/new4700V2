using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControlManager : MonoBehaviour
{
    [Range(0, 1)] public float rC = 0.0f; // red channel
    [Range(0, 1)] public float gC = 0.0f; // green channel
    [Range(0, 1)] public float bC = 0.0f; // blue channel
    [Range(0, 10)] public float gammaC = 0.0f; //gamma

    public float rCThreshold = 0.8f; 
    public GameObject bridge2; 
    public GameObject bridgeCollider; 

    // Start is called before the first frame update
    void Start()
    {
        UpdateBridgeStates();
    }

    void Update()
    {
        UpdateBridgeStates();
    }

    private void UpdateBridgeStates()
    {
        if (bridge2 != null && bridgeCollider != null)
        {
            // Enable bridge2 and disable bridgeCollider if rC exceeds the threshold
            if (rC > rCThreshold)
            {
                bridge2.SetActive(true);
                bridgeCollider.SetActive(false);
            }
            else
            {
                // Reset their states if rC is below the threshold
                bridge2.SetActive(false);
                bridgeCollider.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("bridge2 or bridgeCollider is not assigned in the Inspector.");
        }
    }
}
