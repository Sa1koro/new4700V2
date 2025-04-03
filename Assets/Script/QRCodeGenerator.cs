using UnityEngine;
using ZXing; // ZXing 库
using ZXing.QrCode;
using System;
using System.Security.Cryptography;
using System.Text;

public class QRCodeGenerator : MonoBehaviour
{
    public bool showQRCode = false; 
    public Texture2D qrCodeTexture; // 用于显示生成的二维码
    public string baseUrl = "https://4700.vercel.app/"; // 基础 URL
    public static string uuidMD5;
    private static string sessionGUID;
    private static string fullUrl;
    

    void Awake()
    {
        // 检查是否已经生成 GUID
        if (string.IsNullOrEmpty(sessionGUID))
        {
            Guid guid0 = Guid.NewGuid();
            sessionGUID = guid0.ToString();
            Debug.Log("Generated GUID: " + sessionGUID);
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(guid0.ToString()));
                uuidMD5 = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 8); // 返回8位哈希
            }
        }

        // 拼接 URL
        fullUrl = baseUrl + "?uuid=" + uuidMD5;
    }

    void Start()
    {
        // 生成二维码
        qrCodeTexture = GenerateQRCode(fullUrl);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) // 按下 V 键切换显示状态
        {
            ToggleQRCodeDisplay();
            Debug.Log($"UUID:{uuidMD5}"); 
        }
    }


    Texture2D GenerateQRCode(string text)
    {
        // 定义二维码的宽度和高度
        int width = 256;
        int height = 256;

        // 设置二维码编码选项
        var writer = new QRCodeWriter();
        var bitMatrix = writer.encode(text, BarcodeFormat.QR_CODE, width, height);

        // 创建纹理
        Texture2D texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 根据二维码的像素设置纹理颜色
                texture.SetPixel(x, y, bitMatrix[x, y] ? Color.black : Color.white);
            }
        }

        // 应用像素更改并返回纹理
        texture.Apply();
        return texture;
    }

    void OnGUI()
    {
        // 在屏幕上显示二维码
        if (showQRCode && qrCodeTexture != null)
        {
            GUI.DrawTexture(new Rect(10, 10, 256, 256), qrCodeTexture);
        }
    }

    public void ToggleQRCodeDisplay()
    {
        showQRCode = !showQRCode; // 切换显示状态
    }

    public string getUUID()
    {
        return uuidMD5; // 切换显示状态
    }
}
