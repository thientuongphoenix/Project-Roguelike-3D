using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawnerHaveObjPooling : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private int initialPoolSize = 10; // Số lượng object ban đầu cho mỗi pool

    public WaveHaveObjPooling[] waves;
    public int currentWaveIndex = 0;

    private bool readyToCountdown;
    private bool isGameWon = false;

    private void Start()
    {
        readyToCountdown = true;
        winPanel.SetActive(false); // Ẩn Panel Win ban đầu

        // Khởi tạo pool cho mỗi loại enemy
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemyLeft = waves[i].enemies.Length;
            foreach (Enemy enemy in waves[i].enemies)
            {
                string poolKey = $"Enemy_{enemy.GetType().Name}";
                PoolManager.Instance.CreatePool(poolKey, enemy, initialPoolSize);
            }
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
                // Code mới sử dụng Object Pooling
                string poolKey = $"Enemy_{waves[currentWaveIndex].enemies[i].GetType().Name}";
                Enemy enemy = PoolManager.Instance.GetObject<Enemy>(
                    poolKey,
                    spawnPoint.transform.position,
                    spawnPoint.transform.rotation
                );

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

        AudioManager.Instance.PlaySFX(SoundType.ButtonClick);
    }

    /// <summary>
    /// Quay về Main Menu khi nhấn "Main Menu".
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian game
        SceneManager.LoadScene("MenuScene");

        AudioManager.Instance.PlaySFX(SoundType.ButtonClick);
        AudioManager.Instance.StopMusic();
    }
}

[System.Serializable]
public class WaveHaveObjPooling
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemyLeft; //Số lượng enemy còn sống, số này về 0 sẽ kích hoạt thời gian đếm ngược cho wave tiếp theo
}
