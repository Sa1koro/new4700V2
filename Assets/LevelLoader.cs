using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��������ת����
/// </summary>
public class LevelLoader : MonoBehaviour
{

    private void Start()
    {
        // �ָ�ʱ�������ٶ�Ϊ����ֵ
        Time.timeScale = 1;
    }

    /// <summary>
    /// ��ת��ָ������
    /// </summary>
    public void LoadScene(string sceneName)
    {
        // �첽���س���
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
    }

    /// <summary>
    /// ���¼��ص�ǰ����
    /// </summary>
    public void ReloadScene()
    {
        // ��ȡ��ǰ����ĳ���
        Scene scene = SceneManager.GetActiveScene();

        // �첽���ص�ǰ����
        AsyncOperation op = SceneManager.LoadSceneAsync(scene.name);
    }

    /// <summary>
    /// �˳���Ϸ
    /// </summary>
    public void ExitGame()
    {
        // ����� Unity �༭���У�ֹͣ����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ر�Ӧ�ó���
        Application.Quit();
#endif
    }
}
