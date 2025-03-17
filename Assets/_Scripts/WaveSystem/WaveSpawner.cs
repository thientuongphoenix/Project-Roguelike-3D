using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private GameObject winPanel;

    public Wave[] waves;

    public int currentWaveIndex = 0;

    private bool readyToCountdown;
    private bool isGameWon = false;

    private void Start()
    {
        readyToCountdown = true;
        winPanel.SetActive(false); // Ẩn Panel Win ban đầu

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemyLeft = waves[i].enemies.Length;
        }
    }

    private void Update()
    {
        if (isGameWon) return; // Nếu đã thắng, không chạy Update nữa

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("Hết Wave rồi, Thắng rồi!");
            WinGame(); // Kích hoạt UI Win
            return;
        }

        if (readyToCountdown == true)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountdown = false;

            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
        }

        // Nếu không còn con enemy nào sống thì đi tiếp tới wavw sau
        if (waves[currentWaveIndex].enemyLeft == 0)
        {
            readyToCountdown = true;

            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnWave()
    {
        if(currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                Enemy enemy = Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.transform);

                enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
        
    }

    /// <summary>
    /// Kích hoạt UI Win khi hoàn thành tất cả Waves.
    /// </summary>
    private void WinGame()
    {
        isGameWon = true;
        Time.timeScale = 0f; // Dừng game
        winPanel.SetActive(true); // Hiện UI Win
    }

    /// <summary>
    /// Load lại màn chơi hiện tại khi nhấn "Play Again".
    /// </summary>
    public void PlayAgain()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Load lại Scene hiện tại
    }

    /// <summary>
    /// Quay về Main Menu khi nhấn "Main Menu".
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian game
        SceneManager.LoadScene("MenuScene");
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemyLeft; //Số lượng enemy còn sống, số này về 0 sẽ kích hoạt thời gian đếm ngược cho wave tiếp theo
}