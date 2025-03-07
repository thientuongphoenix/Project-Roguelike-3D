using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3f; // Đạn tồn tại trong 3 giây
    public WeaponStats weaponStats;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Tự hủy sau thời gian nhất định
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Đạn chạm kẻ địch: " + other.name);

            // Kiểm tra Enemy có script EnemyHealth không
            EnemyHealth_Tuong enemyHealth = other.GetComponent<EnemyHealth_Tuong>();
            if (enemyHealth != null)
            {
                int damage = weaponStats.damage;
                enemyHealth.TakeDamage(damage);
                //Debug.Log("Gây sát thương: " + damage);
            }

            Destroy(gameObject); // Hủy đạn sau khi bắn trúng
        }
    }
}
