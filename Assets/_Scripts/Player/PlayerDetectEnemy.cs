using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Quét và theo dõi enemy trong phạm vi DetectionRange của Player.
/// </summary>
public class PlayerDetectEnemy : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LayerMask enemyLayer;
    private List<Transform> detectedEnemies = new List<Transform>();

    private Vector3? closestEnemyPosition = null; // Lưu vị trí enemy gần nhất

    /// <summary>
    /// Danh sách enemy được phát hiện.
    /// </summary>
    public List<Transform> DetectedEnemies => detectedEnemies;

    private void Update()
    {
        DetectEnemies();
    }

    /// <summary>
    /// Quét và cập nhật danh sách enemy trong phạm vi DetectionRange.
    /// </summary>
    private void DetectEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerStats.DetectionRange, enemyLayer);
        detectedEnemies.Clear();

        foreach (Collider collider in hitColliders)
        {
            if (collider.transform != null && collider.gameObject.activeInHierarchy)
            {
                detectedEnemies.Add(collider.transform);
                //Debug.Log("Phát hiện Enemy có vị trí: " + collider.transform);
            }
        }

        // Sắp xếp danh sách theo khoảng cách từ gần đến xa
        detectedEnemies = detectedEnemies.OrderBy(e => Vector3.Distance(transform.position, e.position)).ToList();
    }

    /// <summary>
    /// Lấy enemy gần nhất bằng Raycast từ danh sách đã phát hiện.
    /// </summary>
    /// <returns>Transform của enemy gần nhất hoặc null nếu không có enemy.</returns>
    public Transform GetClosestEnemyWithRaycast()
    {
        closestEnemyPosition = null; // Reset vị trí enemy trước khi quét, cái biến này dùng cho Gizmos vẽ thôi

        foreach (Transform enemy in detectedEnemies)
        {
            if (enemy != null && Physics.Raycast(transform.position, (enemy.position - transform.position).normalized, out RaycastHit hit))
            {
                if (hit.transform == enemy)
                {
                    closestEnemyPosition = enemy.position; // Cập nhật vị trí enemy gần nhất
                    return enemy;
                }
            }
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStats.DetectionRange);

        // Nếu có enemy gần nhất, vẽ đường
        if (closestEnemyPosition.HasValue)
        {
            Gizmos.DrawLine(transform.position, closestEnemyPosition.Value);
        }
    }
}
