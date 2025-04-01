using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 管理场景跳转的类
/// </summary>
public class LevelLoader : MonoBehaviour
{

    private void Start()
    {
        // 恢复时间流逝速度为正常值
        Time.timeScale = 1;
    }

    /// <summary>
    /// 跳转到指定场景
    /// </summary>
    public void LoadScene(string sceneName)
    {
        // 异步加载场景
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
    }

    /// <summary>
    /// 重新加载当前场景
    /// </summary>
    public void ReloadScene()
    {
        // 获取当前激活的场景
        Scene scene = SceneManager.GetActiveScene();

        // 异步加载当前场景
        AsyncOperation op = SceneManager.LoadSceneAsync(scene.name);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        // 如果在 Unity 编辑器中，停止播放
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 关闭应用程序
        Application.Quit();
#endif
    }
}
