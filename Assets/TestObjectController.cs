using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectController : MonoBehaviour
{
    private ColorControlManager colorManager;

    void Start()
    {
        //ColorControlManager
        colorManager = FindObjectOfType<ColorControlManager>();
    }

    void Update()
    {
        if (colorManager != null)
        {
            GetComponent<Renderer>().material.color = new Color(colorManager.rC, colorManager.gC, colorManager.bC);

            //float scaledValue = colorManager.gammaC * 2f;
            //transform.localScale = Vector3.one * scaledValue;
        }
    }
}
