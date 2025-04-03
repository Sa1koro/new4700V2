using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ColorDataFetcher : MonoBehaviour
{
    // Define the DataModel to store the color data
    [System.Serializable]
    public class DataModel
    {
        public float red;
        public float green;
        public float blue;
        public float lightness;
    }
    
    public Light targetLight; // Light object to update
    public ColorControlManager colorManager;
    public DrainableObject drainableObject; // Reference to your color manager

    // URL of your Node.js server
    private string GetFullApiUrl() {
        return $"https://4700.vercel.app/api/getColor?key={QRCodeGenerator.uuidMD5}";
    }
    private float fetchInterval = 30f; // 30 seconds
    private float lastFetchTime = -9999f; // A time in the past to make the first call immediately

    // Start is called before the first frame updatez
    void Start()
    {
        StartCoroutine(FetchColorData());
    }

    void Update()
    {
        // Check if the player presses the 'R' key to manually trigger a refresh
        if (Input.GetKeyDown(KeyCode.R))
        {   
            Debug.Log(GetFullApiUrl()); 
            StartCoroutine(FetchColorData());// Trigger fetch when R is pressed
        }

        // Automatically fetch color data every 30 seconds
        if (Time.time - lastFetchTime >= fetchInterval)
        {
            StartCoroutine(FetchColorData());
            lastFetchTime = Time.time;
        }
    }
    // Fetch color data from the server
    IEnumerator FetchColorData()
    {
        UnityWebRequest request = UnityWebRequest.Get(GetFullApiUrl());
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Log the raw JSON response
            //Debug.Log($"Raw JSON response: {request.downloadHandler.text}");
            // Deserialize the JSON response into the DataModel
            DataModel colorData = JsonUtility.FromJson<DataModel>(request.downloadHandler.text);

            // Apply to Unity objects
            if (targetLight != null)
            {
                targetLight.intensity = colorData.lightness * 10f;
            }
            if (colorManager != null)
            {
                colorManager.rC = colorData.red;
                colorManager.gC = colorData.green;
                colorManager.bC = colorData.blue;
                colorManager.gammaC = colorData.lightness;
            }

            if (drainableObject != null)
            {
                // 如果服务器返回是 0~255，需要先归一化到 0~1
                // 假设后端目前返回的是 0~255，就写:
                float r = colorData.red;
                float g = colorData.green;
                float b = colorData.blue;
                Color newColor = new(r, g, b, 1f);  

                // 调用 FillColor，设置 volumeToAdd = 5（或你想要的其它值）
                // 这样 DrainableObject 就会从白色填充为 newColor。
                bool success = drainableObject.FillColor(newColor, 5);
                drainableObject.initialColor = newColor;
                drainableObject.UpdateSpriteColor();

                if (success)
                {
                    Debug.Log("DrainableObject 填色成功！");
                }
                else
                {
                    Debug.LogWarning("DrainableObject 填色失败，可能已非白色或已存在其他颜色。");
                }
            }

            // Output the received color data
            Debug.Log($"Received color data: Red: {colorData.red}, Green: {colorData.green}, Blue: {colorData.blue}, Lightness: {colorData.lightness}");
        }
        else
        {
            Debug.LogError($"Error fetching color data: {request.error}");
        }
    }
}


