using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebDataFetcher : MonoBehaviour
{
    public string url = "https://4700.vercel.app/"; // URL of the website
    public Light targetLight; // Light object to update
    public ColorControlManager colorManager; // Reference to your color manager

    void Start()
    {
        StartCoroutine(FetchDataFromWeb());
    }

    IEnumerator FetchDataFromWeb()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Parse JSON data
                string json = request.downloadHandler.text;
                ProcessData(json);
            }
            else
            {
                Debug.LogError("Error fetching data: " + request.error);
            }

            yield return new WaitForSeconds(1); // Fetch data every second
        }
    }

    void ProcessData(string json)
    {
        // Parse JSON (requires JSON Utility or Newtonsoft.Json)
        DataModel data = JsonUtility.FromJson<DataModel>(json);

        // Apply to Unity objects
        if (targetLight != null)
        {
            targetLight.intensity = data.gammaC * 10f;
        }
        if (colorManager != null)
        {
            colorManager.gammaC = data.gammaC;
            colorManager.rC = data.rC;
            colorManager.gC = data.gC;
            colorManager.bC = data.bC;
        }
    }
}

[System.Serializable]
public class DataModel
{
    public float gammaC;
    public float rC;
    public float gC;
    public float bC;
}

