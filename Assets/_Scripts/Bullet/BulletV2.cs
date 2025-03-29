using UnityEngine;

/// <summary>
/// Hệ thống xử lý viên đạn.
/// </summary>
public class BulletV2 : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float damage;

    /// <summary>
    /// Thiết lập mục tiêu cho viên đạn.
    /// </summary>
    public void SetTarget(Transform enemyTarget, float bulletSpeed, float bulletDamage)
    {
        target = enemyTarget;
        speed = bulletSpeed;
        damage = bulletDamage;
    }

    void Update()
    {
        if (target == null)
        {
            PoolManager.Instance.ReturnObject<BulletV2>("BulletV2", this);
            return;
        }

        // Di chuyển viên đạn về phía enemy
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            ApplyDamage(other);
            PoolManager.Instance.ReturnObject<BulletV2>("BulletV2", this);
        }
    }

    /// <summary>
    /// Gây sát thương cho enemy.
    /// </summary>
    void ApplyDamage(Collider enemy)
    {
        EnemyHealth_NonNavmeshV2 enemyHealth = enemy.GetComponent<EnemyHealth_NonNavmeshV2>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
