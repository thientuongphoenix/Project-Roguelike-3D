using System.Collections;
using UnityEngine;

public class EnemySpawn_HaveObjPooling : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab; // Prefab của enemy
    public Transform[] spawnPoints; // Danh sách các điểm spawn
    public float spawnInterval = 3f; // Thời gian giữa mỗi lần spawn
    public int maxEnemies = 10; // Số lượng enemy tối đa trên bản đồ
    public int initialPoolSize = 5; // Số lượng enemy ban đầu trong pool

    private int currentEnemyCount = 0;
    private string poolKey;

    private void Start()
    {
        // Khởi tạo pool cho enemy
        poolKey = $"Enemy_{enemyPrefab.name}";
        PoolManager.Instance.CreatePool(poolKey, enemyPrefab.GetComponent<Enemy>(), initialPoolSize);
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Coroutine lặp lại để spawn enemy sau mỗi khoảng thời gian.
    /// </summary>
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    /// <summary>
    /// Sinh ra một enemy tại vị trí ngẫu nhiên trong danh sách spawnPoints.
    /// </summary>
    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogWarning("⚠ Không có điểm spawn hoặc prefab enemy chưa được gán!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy enemy = PoolManager.Instance.GetObject<Enemy>(poolKey, spawnPoint.position, Quaternion.identity);
        
        if (enemy != null)
        {
            currentEnemyCount++;
        }
    }

    /// <summary>
    /// Giảm số lượng enemy hiện tại khi một enemy chết
    /// </summary>
    public void DecreaseEnemyCount()
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
        }
    }
}
