using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pausePanel; // Panel Pause Game
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI shieldText;

    [Header("Player Stats")]
    public PlayerStats playerStats; // ScriptableObject chứa chỉ số Player

    private bool isPaused = false; // Trạng thái game

    private void Start()
    {
        pausePanel.SetActive(false); // Ẩn Panel khi bắt đầu
    }

    /// <summary>
    /// Khi nhấn nút Pause, dừng game và hiển thị UI.
    /// </summary>
    public void PauseGame()
    {
        if (isPaused) return; // Nếu đã pause thì không cần pause nữa

        isPaused = true;
        Time.timeScale = 0f; // Dừng thời gian game
        pausePanel.SetActive(true); // Hiện Panel Pause

        UpdateStatsUI(); // Cập nhật chỉ số
    }

    /// <summary>
    /// Khi nhấn Resume, tiếp tục game.
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Tiếp tục thời gian game
        pausePanel.SetActive(false); // Ẩn Panel Pause
    }

    /// <summary>
    /// Khi nhấn Main Menu, quay về Menu Scene.
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian trước khi load scene
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Cập nhật UI hiển thị chỉ số của Player.
    /// </summary>
    private void UpdateStatsUI()
    {
        speedText.text = $"Speed: {playerStats.Speed}";
        healthText.text = $"Health: {playerStats.Health}/{playerStats.MaxHealth}";
        armorText.text = $"Armor: {playerStats.Armor}";
        shieldText.text = $"Shield: {playerStats.Shield}/{playerStats.MaxShield}";
    }
}
