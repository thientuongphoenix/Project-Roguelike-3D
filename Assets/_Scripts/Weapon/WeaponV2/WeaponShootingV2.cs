using UnityEngine;

/// <summary>
/// Hệ thống bắn vũ khí tự động.
/// </summary>
public class WeaponShootingV2 : MonoBehaviour
{
    public Transform firePoint;
    public WeaponStats weaponStats;
    private WeaponDetectionV2 detectionSystem;
    private float nextFireTime;

    public BulletV2 bulletPrefab;

    private const string BULLET_POOL_KEY = "BulletV2";
    private const int INITIAL_BULLET_COUNT = 10;

    void Start()
    {
        detectionSystem = GetComponent<WeaponDetectionV2>();

        PoolManager.Instance.CreatePool(BULLET_POOL_KEY, bulletPrefab, INITIAL_BULLET_COUNT);
    }

    void Update()
    {
        ShootAtTarget();
    }

    /// <summary>
    /// Kiểm tra nếu đủ cooldown thì bắn đạn.
    /// </summary>
    void ShootAtTarget()
    {
        if (Time.time < nextFireTime) return;

        Transform target = detectionSystem.GetTarget();
        if (target == null) return;

        Fire(target);
        nextFireTime = Time.time + 1f / weaponStats.fireRate;
    }

    /// <summary>
    /// Tạo viên đạn và bắn về hướng enemy.
    /// </summary>
    void Fire(Transform target)
    {
        if (weaponStats.bulletPrefab == null) return;

        // Xác định hướng từ súng đến enemy
        Vector3 direction = (target.position - firePoint.position).normalized;

        // Tạo góc quay cho viên đạn hướng về enemy
        Quaternion bulletRotation = Quaternion.LookRotation(direction);

        // Điều chỉnh góc quay nếu cần (nếu viên đạn vẫn chưa đúng)
        bulletRotation *= Quaternion.Euler(90, 0, 0);

        // Tạo viên đạn với góc quay chính xác
        //GameObject bullet = Instantiate(weaponStats.bulletPrefab, firePoint.position, bulletRotation);
        var bullet = PoolManager.Instance.GetObject<BulletV2>(BULLET_POOL_KEY, firePoint.position, bulletRotation);
        
        BulletV2 bulletScript = bullet.GetComponent<BulletV2>();

        AudioManager.Instance.PlaySFX(SoundType.PlayerShoot);

        if (bulletScript != null)
        {
            bulletScript.SetTarget(target, weaponStats.bulletSpeed, weaponStats.damage);
        }
    }
}
