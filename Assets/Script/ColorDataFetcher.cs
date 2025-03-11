using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ColorDataFetcherL2 : MonoBehaviour
{
    [System.Serializable]
    public class DataModel
    {
        public float red;
        public float green;
        public float blue;
        public float lightness;
    }

    [SerializeField] private Vector4 fetchedColor; // Visible in Inspector

    private string GetFullApiUrl() {
        return $"https://4700.vercel.app/api/getColor?key={QRCodeGenerator.uuidMD5}";
    }

    void Start()
    {
        StartCoroutine(FetchColorData());
    }

    IEnumerator FetchColorData()
    {
        UnityWebRequest request = UnityWebRequest.Get(GetFullApiUrl());
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            DataModel colorData = JsonUtility.FromJson<DataModel>(request.downloadHandler.text);
            fetchedColor = new Vector4(colorData.red, colorData.green, colorData.blue, colorData.lightness);
        }
        else
        {
            Debug.LogError($"Error fetching color data: {request.error}");
        }
    }
}
