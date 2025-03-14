using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth_NonNavmesh : MonoBehaviour
{
    public EnemyStats_Tuong baseStats; // Gán từ Inspector (Dữ liệu gốc)
    public EnemyStats_Tuong enemyStats; // 🔥 Bản sao dành riêng cho từng enemy
    public PlayerStats playerStats; // 🔥 Gán trực tiếp PlayerStats
    public PlayerExp playerExp; // 🔥 Dùng để cập nhật UI sau khi cộng EXP
    public GameObject dropItemPrefab; // Prefab vật phẩm rớt ra khi chết
    public EnemyMovement enemyMovement;

    private EnemyAnimationController_Tuong enemyAnim;
    public bool isDead {  get; private set; }

    private void Awake()
    {
        // 🔥 Tạo bản sao thay vì dùng trực tiếp ScriptableObject
        enemyStats = Instantiate(baseStats);
        enemyMovement = GetComponent<EnemyMovement>(); // 🔥 Đảm bảo luôn có EnemyMovement
    }

    void Start()
    {
        enemyStats.health = enemyStats.maxHealth;

        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
        isDead = false;
    }

    void OnEnable()
    {
        isDead = false; // 🔥 Reset lại trạng thái chết khi enemy được tái sử dụng
        enemyStats.health = enemyStats.maxHealth;
        //enemyMovement.enabled = true; // 🔥 Bật lại di chuyển khi enemy hồi sinh
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log(isDead);
        if (isDead) return; // 🔥 Tránh nhận damage sau khi đã chết

        enemyStats.health -= damage;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Damage);

        if (enemyStats.health <= 0)
        {
            
            enemyStats.health = 0;
            StartCoroutine(Die());
            //isDead = false;
            //enemyStats.health = enemyStats.maxHealth;
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        //enemyMovement.enabled = false;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Die); // Phát animation chết
        
        Debug.Log("Trạng thái chết đang chạy!");

        // 🔥 Cộng EXP trực tiếp vào PlayerStats
        playerStats.TotalExp += enemyStats.expReward;

        // 🔥 Cập nhật UI của PlayerExp (nếu có)
        if (playerExp != null)
        {
            playerExp.CheckLevelUp();
            playerExp.UpdateUI();
        }

        // 🔍 Lấy thời gian animation thực tế từ Animator
        Animator animator = GetComponent<Animator>();

        // 🔥 Chờ đến khi animation Die thực sự bắt đầu
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            yield return null;
        }

        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathAnimationLength); // 🔥 Chờ đúng thời gian animation

        // Rớt vật phẩm sau khi chết
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        // Trả Enemy về Pool thay vì Destroy
        EnemyPool_Tuong.Instance.ReturnEnemy(gameObject);
    }
}
