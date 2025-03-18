using System.Collections;
using UnityEngine;

/// <summary>
/// Script sinh ra enemy tại các vị trí spawn.
/// </summary>
public class EnemySpawn_TEST : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab; // Prefab của enemy
    public Transform[] spawnPoints; // Danh sách các điểm spawn
    public float spawnInterval = 3f; // Thời gian giữa mỗi lần spawn
    public int maxEnemies = 10; // Số lượng enemy tối đa trên bản đồ

    private int currentEnemyCount = 0;

    private void Start()
    {
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
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemyCount++;
    }
}
