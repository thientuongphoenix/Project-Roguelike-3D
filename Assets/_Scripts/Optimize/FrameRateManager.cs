using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public int targetFrameRate = 120; // Set your desired frame rate here

    void Awake()
    {
        // Tắt V-Sync để Application.targetFrameRate hoạt động
        QualitySettings.vSyncCount = 0;

        // Giới hạn FPS
        Application.targetFrameRate = targetFrameRate;
    }

    // Đảm bảo chỉ có một FrameRateManager trong toàn game
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
