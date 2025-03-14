using UnityEngine;

/// <summary>
/// Hệ thống phát hiện mục tiêu bằng Raycast để tránh lỗi đạn đi xuyên enemy.
/// </summary>
public class WeaponDetectionV2 : MonoBehaviour
{
    public Transform firePoint;
    public LayerMask enemyLayer;

    private PlayerDetectEnemy playerDetectEnemy;
    private Transform target;
    private Vector3? hitPosition = null; // Lưu vị trí va chạm của Raycast

    void Start()
    {
        playerDetectEnemy = Object.FindFirstObjectByType<PlayerDetectEnemy>();
    }

    void Update()
    {
        DetectTarget();
    }

    /// <summary>
    /// Xác định enemy gần nhất bằng Raycast.
    /// </summary>
    void DetectTarget()
    {
        hitPosition = null; // Biến chỉ dùng để vẽ thôi nha má

        if (playerDetectEnemy == null) return;

        Transform closestEnemy = playerDetectEnemy.GetClosestEnemyWithRaycast();

        if (closestEnemy != null)
        {
            // Thực hiện Raycast từ firePoint đến enemy để đảm bảo đường đạn không bị cản
            if (Physics.Raycast(firePoint.position, closestEnemy.position - firePoint.position, out RaycastHit hit, Mathf.Infinity, enemyLayer))
            {
                target = hit.transform;
                hitPosition = hit.point; // Lưu vị trí hit của Raycast
            }
        }
    }

    /// <summary>
    /// Trả về mục tiêu gần nhất.
    /// </summary>
    public Transform GetTarget()
    {
        return target;
    }

    /// <summary>
    /// Vẽ Gizmos để debug Raycast từ firePoint đến enemy.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hitPosition.HasValue)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(firePoint.position, hitPosition.Value);
        }
    }
}
