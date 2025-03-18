using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Tham chiếu đến Panel Game Over

    private PlayerHealth playerHealth;
    private bool isDead;
    

    private void Start()
    {
        playerHealth = GameObject.FindFirstObjectByType<PlayerHealth>();
        isDead = playerHealth.isDead;
    }

    private void FixedUpdate()
    {
        isDead = playerHealth.isDead;
        if (isDead)
        {
            StartCoroutine(WaitOneSecond());
            
            //ShowGameOver();
        }
    }

    /// <summary>
    /// Kích hoạt Game Over Panel khi Player chết.
    /// </summary>
    public void ShowGameOver()
    {
        AudioManager.Instance.PlaySFX(SoundType.GameOver);
        Time.timeScale = 0; // Dừng thời gian game trước khi hiện panel game over
        gameOverPanel.SetActive(true); // Hiện Panel Game Over
    }

    /// <summary>
    /// Load lại scene hiện tại từ đầu.
    /// </summary>
    public void PlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // Bỏ dừng thời gian game

        AudioManager.Instance.PlaySFX(SoundType.ButtonClick);
    }

    /// <summary>
    /// Coroutine chờ 1 giây trước khi tiếp tục.
    /// </summary>
    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1f);
        ShowGameOver();
    }
}
