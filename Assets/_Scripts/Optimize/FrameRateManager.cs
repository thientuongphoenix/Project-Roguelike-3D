using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public int targetFrameRate = 120;

    void Awake()
    {
        // Tắt V-Sync để Application.targetFrameRate hoạt động
        QualitySettings.vSyncCount = 0;

        // Giới hạn FPS
        Application.targetFrameRate = targetFrameRate;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
