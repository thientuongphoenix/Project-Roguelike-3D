using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth_NonNavmesh : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats;
    public PlayerStats playerStats; // 🔥 Gán trực tiếp PlayerStats
    public PlayerExp playerExp; // 🔥 Dùng để cập nhật UI sau khi cộng EXP
    public GameObject dropItemPrefab; // Prefab vật phẩm rớt ra khi chết

    private EnemyAnimationController_Tuong enemyAnim;
    private bool isDead = false;

    void Start()
    {
        enemyStats.health = enemyStats.maxHealth;
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 🔥 Tránh nhận damage sau khi đã chết

        enemyStats.health -= damage;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Damage);

        if (enemyStats.health <= 0)
        {
            enemyStats.health = 0;
            StartCoroutine(Die());
            isDead = false;
        }

        //if (enemyStats.health <= 0)
        //{
        //    enemyStats.health = 0;
        //    enemyAnim.ChangeAnimationState(EnemyAnimationState.Die);

        //    // 🔥 Cộng EXP trực tiếp vào PlayerStats
        //    playerStats.TotalExp += enemyStats.expReward;

        //    // 🔥 Cập nhật UI của PlayerExp (nếu có)
        //    if (playerExp != null)
        //    {
        //        playerExp.CheckLevelUp();
        //        playerExp.UpdateUI();
        //    }

        //    // Rớt vật phẩm sau khi chết
        //    if (dropItemPrefab != null)
        //    {
        //        Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        //    }

        //    StartCoroutine(WaitOneSecond());
        //    EnemyPool_Tuong.Instance.ReturnEnemy(gameObject);
        //}
    }

    IEnumerator Die()
    {
        isDead = true;
        enemyAnim.ChangeAnimationState(EnemyAnimationState.Die); // Phát animation chết

        // 🔥 Cộng EXP trực tiếp vào PlayerStats
        playerStats.TotalExp += enemyStats.expReward;

        // 🔥 Cập nhật UI của PlayerExp (nếu có)
        if (playerExp != null)
        {
            playerExp.CheckLevelUp();
            playerExp.UpdateUI();
        }

        // Chờ cho animation chết diễn ra hoàn thành
        float deathAnimationLength = 1f; // Điều chỉnh thời gian theo animation thực tế
        yield return new WaitForSeconds(deathAnimationLength);

        // Rớt vật phẩm sau khi chết
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        // Trả Enemy về Pool thay vì Destroy
        EnemyPool_Tuong.Instance.ReturnEnemy(gameObject);
    }

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("✅ Đã chờ 1 giây!");
    }
}
