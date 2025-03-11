using System.Collections;
using UnityEngine;

public class EnemySpawner_Tuong : MonoBehaviour
{
    public Transform[] spawnPoints; // Các điểm spawn Enemy
    public float spawnRate = 3f; // Tốc độ spawn
    public float safeDistance = 5f; // Khoảng cách tối thiểu để tránh spawn gần Player
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Không tìm thấy Player! Hãy kiểm tra lại tag của Player.");
            return;
        }

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            Transform validSpawnPoint = GetValidSpawnPoint();
            if (validSpawnPoint != null)
            {
                GameObject enemy = EnemyPool_Tuong.Instance.GetEnemy(validSpawnPoint.position, Quaternion.identity);
                //if (enemy != null)
                //{
                //    Debug.Log("Enemy Spawned tại " + validSpawnPoint.position);
                //}
            }
        }
    }

    Transform GetValidSpawnPoint()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            float distanceToPlayer = Vector3.Distance(spawnPoint.position, player.position);

            if (distanceToPlayer > safeDistance) // Kiểm tra nếu Player không ở gần
            {
                return spawnPoint; // Chọn điểm spawn hợp lệ đầu tiên tìm thấy
            }
        }

        Debug.LogWarning("Không tìm thấy điểm spawn hợp lệ! Chờ đợt spawn tiếp theo.");
        return null; // Không spawn nếu không có điểm hợp lệ
    }
}
