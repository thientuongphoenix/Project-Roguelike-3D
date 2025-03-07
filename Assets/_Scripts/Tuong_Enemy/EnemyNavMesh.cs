using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private EnemyAnimationController_Tuong enemyAnim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();

        // Kiểm tra nếu Agent chưa được đặt trên NavMesh, thì tìm vị trí hợp lệ gần nhất
        if (!agent.isOnNavMesh)
        {
            RepositionToNavMesh();
        }
    }

    void Update()
    {
        CheckNavMeshPosition(); // Kiểm tra nếu Enemy bị rơi khỏi NavMesh

        if (player != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }

        if (agent.velocity.magnitude > 0.1f)
        {
            enemyAnim.ChangeAnimationState(EnemyAnimationState.Move);
        }
        //else
        //{
        //    enemyAnim.ChangeAnimationState(EnemyAnimationState.Idle);
        //}
    }

    void CheckNavMeshPosition()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is off NavMesh! Trying to reposition...");
            RepositionToNavMesh();
        }
    }

    void RepositionToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position; // Dịch Enemy về vị trí hợp lệ
            agent.Warp(hit.position); // Đưa AI trở lại NavMesh
            Debug.Log($"{gameObject.name} repositioned to valid NavMesh area.");
        }
        else
        {
            Debug.LogError($"{gameObject.name} could not find a valid NavMesh position!");
        }
    }
}
