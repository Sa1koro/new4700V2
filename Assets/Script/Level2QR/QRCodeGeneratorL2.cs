using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Security.Cryptography;
using System.Text;
using ZXing;
using ZXing.QrCode;
using System.Collections;

public class QRCodeColorManagerL2 : MonoBehaviour
{
    [Header("QR Code & Color Settings")]
    public string baseUrl = "https://4700.vercel.app/";
    public static string uuidMD5;
    private static string fullUrl;
    
    [SerializeField] private SpriteRenderer qrSpriteRenderer; // Assign in Unity
    [SerializeField] private SpriteRenderer colorSpriteRenderer; // Assign in Unity
    
    public Vector4 fetchedColor; // Display in Inspector
    [SerializeField] private float fetchInterval = 5f; // Fetch color data every 30s

    void Awake()
    {
        GenerateUUID();
        ApplyQRCodeToSprite();
        StartCoroutine(FetchColorDataRoutine());
    }

    void GenerateUUID()
    {
        if (string.IsNullOrEmpty(uuidMD5))
        {
            Guid guid0 = Guid.NewGuid();
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(guid0.ToString()));
                uuidMD5 = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 8);
            }
        }
        fullUrl = $"{baseUrl}?uuid={uuidMD5}";
    }

    void ApplyQRCodeToSprite()
    {
        Texture2D qrTexture = GenerateQRCode(fullUrl);
        if (qrTexture != null && qrSpriteRenderer != null)
        {
            qrSpriteRenderer.sprite = TextureToSprite(qrTexture);
        }
    }

    IEnumerator FetchColorDataRoutine()
    {
        while (true)
        {
            yield return FetchColorData();
            yield return new WaitForSeconds(fetchInterval);
        }
    }

    IEnumerator FetchColorData()
    {
        // Ensure the UUID is generated before making a request
        while (string.IsNullOrEmpty(uuidMD5))
        {
            Debug.LogWarning("UUID is not yet generated, waiting...");
            yield return new WaitForSeconds(1); // Wait 1 second before retrying
        }

        string apiUrl = $"https://4700.vercel.app/api/getColor?key={uuidMD5}";
        Debug.Log($"Fetching color data from: {apiUrl}");

        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Raw Response: {request.downloadHandler.text}");

            ColorData colorData = JsonUtility.FromJson<ColorData>(request.downloadHandler.text);
            fetchedColor = new Vector4(colorData.red, colorData.green, colorData.blue, colorData.lightness);

            if (colorSpriteRenderer != null)
            {
                colorSpriteRenderer.color = new Color(colorData.red, colorData.green, colorData.blue, 1f);
            }
        }
        else
        {
            Debug.LogError($"Error fetching color data: {request.error} (URL: {apiUrl})");
        }
    }

    Texture2D GenerateQRCode(string text)
    {
        int width = 256;
        int height = 256;
        var writer = new QRCodeWriter();
        var bitMatrix = writer.encode(text, BarcodeFormat.QR_CODE, width, height);

        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                texture.SetPixel(x, y, bitMatrix[x, y] ? Color.black : Color.white);
            }
        }
        texture.Apply();
        return texture;
    }

    Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    [System.Serializable]
    public class ColorData
    {
        public float red;
        public float green;
        public float blue;
        public float lightness;
    }
}
