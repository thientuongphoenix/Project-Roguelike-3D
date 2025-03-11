using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool_Tuong : MonoBehaviour
{
    public static EnemyPool_Tuong Instance; // Singleton cho Pool

    [Header("Enemy Pool Settings")]
    public GameObject enemyPrefab; // Prefab của Enemy
    public int poolSize; // Số lượng Enemy tối đa

    private Queue<GameObject> enemyPool = new Queue<GameObject>(); // Hàng đợi lưu trữ Enemy

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Tạo sẵn Pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false); // Ẩn Enemy khi mới tạo
            enemyPool.Enqueue(enemy);
        }
    }

    // Lấy Enemy từ Pool
    public GameObject GetEnemy(Vector3 position, Quaternion rotation)
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue(); // Lấy từ hàng đợi
            enemy.transform.position = position;
            enemy.transform.rotation = rotation;
            enemy.SetActive(true);

            // Reset NavMeshAgent để đảm bảo Enemy hoạt động đúng
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = true;
                agent.isStopped = false;
                agent.Warp(position); // Đưa Enemy vào đúng NavMesh
            }

            // Reset lại máu khi spawn
            EnemyHealth_Tuong enemyHealth = enemy.GetComponent<EnemyHealth_Tuong>();
            if (enemyHealth != null)
            {
                enemyHealth.enemyStats.health = enemyHealth.enemyStats.maxHealth;
            }

            return enemy;
        }
        else
        {
            Debug.LogWarning("Hết Enemy trong Pool! Tăng PoolSize nếu cần.");
            return null;
        }
    }

    // Trả Enemy về Pool (Khi chết)
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy); // Thêm lại vào hàng đợi
    }
}
