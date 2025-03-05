using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGammaController : MonoBehaviour
{
    public Light lightSource;
    private ColorControlManager colorManager;
    public float lampIntenseMulti = 10f;

    void Start()
    {
        colorManager = FindObjectOfType<ColorControlManager>();
    }

    void Update()
    {
        if (lightSource != null && colorManager != null)
        {
            lightSource.intensity = colorManager.gammaC * 10f;
        }
    }
}

