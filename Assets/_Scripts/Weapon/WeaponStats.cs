using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Weapons/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [Header("Basic Stats")]
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;

    [Header("Weapon Stats")]
    public float range; // Tầm bắn
    public float fireRate; // Tốc độ bắn (số phát bắn mỗi giây)
    public float detectionRange; // Tầm phát hiện enemy
    public float damage; // Sát thương
    public float CritChance;
    public float CritDamage;
    public float fireCooldown;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
}
