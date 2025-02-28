using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    public Transform firePoint; // Vị trí đầu súng
    private WeaponDetection detectionSystem;
    public WeaponStats weaponStats;

    private void Start()
    {
        detectionSystem = GetComponentInParent<WeaponDetection>();

        if (detectionSystem == null)
        {
            Debug.LogError("WeaponShooting: Không tìm thấy WeaponDetection!");
        }
    }

    private void Update()
    {
        weaponStats.fireCooldown -= Time.deltaTime;

        Transform target = detectionSystem.GetClosestEnemy();
        if (target != null && weaponStats.fireCooldown <= 0)
        {
            Fire(target);
            weaponStats.fireCooldown = 1f / weaponStats.fireRate; // Reset cooldown
        }
    }

    void Fire(Transform target)
    {
        GameObject bullet = Instantiate(weaponStats.bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            rb.linearVelocity = direction * weaponStats.bulletSpeed;
        }

        Debug.Log("Bắn vào: " + target.name);
    }
}
