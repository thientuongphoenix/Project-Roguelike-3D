using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttack_Tuong : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats; // G�n ScriptableObject ch?a th�ng tin Enemy
    public Transform attackPoint; // ?i?m trung t�m v�ng t?n c�ng
    public LayerMask playerLayer; // Layer c?a Player

    private bool canAttack = true;
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemyStats.attackRange && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        Debug.Log(gameObject.name + " t?n c�ng Player!");

        // Ki?m tra xem Player c� trong t?m ?�nh kh�ng
        Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, enemyStats.attackRange, playerLayer);
        foreach (Collider playerCollider in hitPlayers)
        {
            PlayerHealth playerHealth = playerCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyStats.damage);
                Debug.Log("G�y " + enemyStats.damage + " s�t th??ng l�n Player");
            }
        }

        yield return new WaitForSeconds(enemyStats.attackCooldown);
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, enemyStats.attackRange);
        }
    }
}
