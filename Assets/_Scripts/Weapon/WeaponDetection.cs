using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour
{
    public WeaponStats weaponStats;
    public LayerMask enemyLayer; // Chỉ phát hiện kẻ địch
    private List<Transform> detectedEnemies = new List<Transform>();

    private void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, weaponStats.detectionRange, enemyLayer);
        detectedEnemies.Clear();

        foreach (Collider collider in hitColliders)
        {
            detectedEnemies.Add(collider.transform);
            //Debug.Log("Phát hiện kẻ địch!" + hitColliders.Length);
        }

        //Debug.Log($"[WeaponDetection] Số enemy phát hiện: {detectedEnemies.Count}");
    }

    public Transform GetClosestEnemy()
    {
        CleanUpDeadEnemies(); // Xóa Enemy chết trước khi tìm mục tiêu

        if (detectedEnemies.Count == 0) return null;

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (Transform enemy in detectedEnemies)
        {
            // Kiểm tra Enemy có `EnemyHealth_Tuong` không
            EnemyHealth_Tuong enemyHealth = enemy.GetComponent<EnemyHealth_Tuong>();
            if (enemyHealth != null && enemyHealth.enemyStats.health > 0) // Chỉ chọn Enemy còn sống
            {
                float distance = Vector3.Distance(position, enemy.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        return closestEnemy;
    }

    void CleanUpDeadEnemies()
    {
        for (int i = detectedEnemies.Count - 1; i >= 0; i--)
        {
            Transform enemy = detectedEnemies[i];

            // Kiểm tra Enemy có bị xóa, vô hiệu hóa hoặc đã chết không
            if (enemy == null || !enemy.gameObject.activeInHierarchy)
            {
                detectedEnemies.RemoveAt(i);
                continue;
            }

            EnemyHealth_Tuong enemyHealth = enemy.GetComponent<EnemyHealth_Tuong>();
            if (enemyHealth != null && enemyHealth.enemyStats.health <= 0)
            {
                detectedEnemies.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponStats.detectionRange);
    }
}
