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
        if (weaponStats.bulletPrefab == null)
        {
            Debug.LogError("WeaponShooting: Chưa gán Bullet Prefab trong WeaponStats!");
            return;
        }

        // Tính hướng từ súng đến mục tiêu
        Vector3 direction = (target.position - firePoint.position).normalized;

        // Xoay đạn đúng hướng mục tiêu
        Quaternion bulletRotation = Quaternion.LookRotation(direction);
        bulletRotation *= Quaternion.Euler(-90, 0, 0); // Điều chỉnh góc nếu cần

        // Tạo viên đạn với góc xoay đúng
        GameObject bullet = Instantiate(weaponStats.bulletPrefab, firePoint.position, bulletRotation);

        // Gán vận tốc cho viên đạn
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * weaponStats.bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet Prefab không có Rigidbody!");
        }

        Debug.Log("Bắn vào: " + target.name);
    }
}
