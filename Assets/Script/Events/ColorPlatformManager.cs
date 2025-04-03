using System.Collections;
using UnityEngine;

public class ColorPlatformManager : MonoBehaviour
{
    [Header("Drainable Object Targets")]
    public DrainableObject redTarget;
    public DrainableObject greenTarget;
    public DrainableObject blueTarget;

    [Header("Platform References")]
    public Transform redPlatform;
    public Transform greenPlatform;
    public Transform bluePlatform;

    [Header("Height Settings")]
    // Individual min/max height for each platform
    public float redMinHeight = 96f, redMaxHeight = 102f;
    public float greenMinHeight = 99f, greenMaxHeight = 105f;
    public float blueMinHeight = 102f, blueMaxHeight = 108f;

    public float colorThresholdMin = 10f;
    public float colorThresholdMax = 60f;
    public float moveSpeed = 2f; // Speed of platform animation
    public float defaultHeight = 96f;

    private float redTargetHeight;
    private float greenTargetHeight;
    private float blueTargetHeight;

    private void Start()
    {
        // Ensure platforms start at their minimum height
        if (redPlatform != null) redPlatform.localPosition = new Vector3(redPlatform.localPosition.x, defaultHeight, redPlatform.localPosition.z);
        if (greenPlatform != null) greenPlatform.localPosition = new Vector3(greenPlatform.localPosition.x, defaultHeight, greenPlatform.localPosition.z);
        if (bluePlatform != null) bluePlatform.localPosition = new Vector3(bluePlatform.localPosition.x, defaultHeight, bluePlatform.localPosition.z);
    }

    private void Update()
    {
        if (redTarget != null) redTargetHeight = CalculateHeight(redTarget.initialColor, Color.red, redMinHeight, redMaxHeight);
        if (greenTarget != null) greenTargetHeight = CalculateHeight(greenTarget.initialColor, Color.green, greenMinHeight, greenMaxHeight);
        if (blueTarget != null) blueTargetHeight = CalculateHeight(blueTarget.initialColor, Color.blue, blueMinHeight, blueMaxHeight);

        // Move platforms smoothly
        MovePlatformTowardsTarget(redPlatform, redTargetHeight);
        MovePlatformTowardsTarget(greenPlatform, greenTargetHeight);
        MovePlatformTowardsTarget(bluePlatform, blueTargetHeight);
    }

    private float CalculateHeight(Color objectColor, Color referenceColor, float minHeight, float maxHeight)
    {
        float colorDistance = Mathf.Sqrt(
            Mathf.Pow(objectColor.r * 255 - referenceColor.r * 255, 2) +
            Mathf.Pow(objectColor.g * 255 - referenceColor.g * 255, 2) +
            Mathf.Pow(objectColor.b * 255 - referenceColor.b * 255, 2)
        );

        // 🛠 If color is white (default state), return defaultHeight
        if (objectColor == Color.white) return defaultHeight;

        if (colorDistance >= colorThresholdMax) return minHeight;
        if (colorDistance <= colorThresholdMin) return maxHeight;

        return minHeight + (maxHeight - minHeight) * ((colorThresholdMax - colorDistance) / (colorThresholdMax - colorThresholdMin));
    }


    private void MovePlatformTowardsTarget(Transform platform, float targetHeight)
    {
        if (platform == null) return;

        float currentHeight = platform.localPosition.y;

        if (Mathf.Abs(currentHeight - targetHeight) > 0.01f) // Only move if there's a meaningful difference
        {
            float newHeight = Mathf.MoveTowards(currentHeight, targetHeight, moveSpeed * Time.deltaTime);
            platform.localPosition = new Vector3(platform.localPosition.x, newHeight, platform.localPosition.z);
        }
    }
}
