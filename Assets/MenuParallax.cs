using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    // ƫ�Ʊ����������Ӳ�Ч����ǿ��
    public float offsetMultiplier = 1f;

    // ƽ���ƶ�ʱ�䣬����ƽ������Ч��
    public float smoothTime = 0.3f;

    // ��¼��ʼλ��
    private Vector2 startPosition;

    // ��¼�ƶ��ٶȣ�����ƽ���ƶ����㣩
    private Vector3 velocity;

    private void Start()
    {
        // ��¼����ĳ�ʼλ��
        startPosition = transform.position;
    }

    private void Update()
    {
        // ��ȡ�������Ļ�ϵ�λ�ã���ת��Ϊ�ӿ����꣨0~1 ��Χ��
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // ���ﻹ��Ҫʹ�� offset �����������λ�ñ任
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + (offset * offsetMultiplier), ref velocity, smoothTime);
    }
}
