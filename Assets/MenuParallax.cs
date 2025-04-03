using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    // 偏移倍数，控制视差效果的强度
    public float offsetMultiplier = 1f;

    // 平滑移动时间，用于平滑过渡效果
    public float smoothTime = 0.3f;

    // 记录初始位置
    private Vector2 startPosition;

    // 记录移动速度（用于平滑移动计算）
    private Vector3 velocity;

    private void Start()
    {
        // 记录物体的初始位置
        startPosition = transform.position;
    }

    private void Update()
    {
        // 获取鼠标在屏幕上的位置，并转换为视口坐标（0~1 范围）
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // 这里还需要使用 offset 来计算物体的位置变换
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + (offset * offsetMultiplier), ref velocity, smoothTime);
    }
}
