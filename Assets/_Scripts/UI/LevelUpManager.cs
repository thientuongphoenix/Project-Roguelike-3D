using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public PlayerStats playerStats; // Tham chiếu đến PlayerStats
    public GameObject shopPanel; // Panel shop
    public Button nextButton; // Nút Next để tiếp tục game
    private float lastLevel; // Lưu trữ level trước đó

    void Start()
    {
        lastLevel = playerStats.Level; // Lưu level ban đầu
        shopPanel.SetActive(false); // Ẩn shop khi game bắt đầu

        // Gán sự kiện cho nút Next
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(ContinueGame);
        }
    }

    void Update()
    {
        // Kiểm tra nếu Level của Player đã tăng
        if (playerStats.Level > lastLevel)
        {
            PauseGame(); // Dừng game khi lên level
            lastLevel = playerStats.Level; // Cập nhật level mới
        }
    }

    /// <summary>
    /// Dừng game và hiển thị Shop Panel khi lên level.
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0f; // Dừng thời gian trong game
        shopPanel.SetActive(true); // Hiển thị Shop Panel
    }

    /// <summary>
    /// Tiếp tục game khi ấn Next.
    /// </summary>
    private void ContinueGame()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian trong game
        shopPanel.SetActive(false); // Ẩn Shop Panel
    }
}
