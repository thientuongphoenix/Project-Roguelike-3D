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
    }

    public Transform GetClosestEnemy()
    {
        if (detectedEnemies.Count == 0) return null;

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (Transform enemy in detectedEnemies)
        {
            float distance = Vector3.Distance(position, enemy.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponStats.detectionRange);
    }
}
