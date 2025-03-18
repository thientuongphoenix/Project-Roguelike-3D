using System.Collections;
using UnityEngine;

/// <summary>
/// Xử lý máu và trạng thái của enemy không sử dụng NavMesh. Đồng thời dùng riêng chỉ số máu, tránh tình trạng 1 enemy chết thì tất cả đều chết.
/// </summary>
public class EnemyHealth_NonNavmeshV2 : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 100f; // Máu tối đa của enemy
    [SerializeField] private float currentHealth; // Máu hiện tại của enemy
    public float expReward = 50f; // EXP nhận được khi tiêu diệt enemy

    [Header("References")]
    public PlayerStats playerStats; // Tham chiếu đến thông tin Player
    public PlayerExp playerExp; // Tham chiếu đến hệ thống EXP của Player
    public GameObject dropItemPrefab; // Prefab vật phẩm rớt ra khi enemy chết
    private EnemyAnimationController_Tuong enemyAnim; // Bộ điều khiển animation

    private WaveSpawner waveSpawner;

    public bool IsDead { get; private set; } // Trạng thái của enemy

    private void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
        currentHealth = maxHealth; // Đặt máu ban đầu
        IsDead = false;

        waveSpawner = GetComponentInParent<WaveSpawner>();
    }

    /// <summary>
    /// Enemy nhận damage và kiểm tra trạng thái chết.
    /// </summary>
    /// <param name="damage">Lượng sát thương nhận vào.</param>
    public void TakeDamage(float damage)
    {
        if (IsDead) return; // Không nhận damage nếu đã chết

        currentHealth -= damage;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Damage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            enemyAnim.ChangeAnimationState(EnemyAnimationState.Die); // Phát animation chết
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Coroutine xử lý trạng thái chết của enemy.
    /// </summary>
    IEnumerator Die()
    {
        IsDead = true;
        //enemyAnim.ChangeAnimationState(EnemyAnimationState.Die); // Phát animation chết

        Debug.Log("Enemy chết!");

        // Cộng EXP trực tiếp vào PlayerStats
        playerStats.TotalExp += expReward;

        // Cập nhật UI của PlayerExp (nếu có)
        if (playerExp != null)
        {
            playerExp.CheckLevelUp();
            playerExp.UpdateUI();
        }

        // Rớt vật phẩm sau khi chết
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);

        // Hủy enemy khỏi scene
        Destroy(gameObject);

        //Giảm số lượng enemy còn sống trong Wave Spawner
        waveSpawner.waves[waveSpawner.currentWaveIndex].enemyLeft--;
    }
}
