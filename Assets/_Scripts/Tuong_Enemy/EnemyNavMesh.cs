using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public EnemyStats_Tuong enemyStats;

    public Transform player;
    private NavMeshAgent agent;
    private EnemyAnimationController_Tuong enemyAnim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<EnemyAnimationController_Tuong>();
        //player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Kiểm tra nếu Agent chưa được đặt trên NavMesh, thì tìm vị trí hợp lệ gần nhất
        if (!agent.isOnNavMesh)
        {
            RepositionToNavMesh();
        }
    }

    void Update()
    {
        //// 🛑 Kiểm tra nếu Enemy đã chết, không gọi SetDestination
        //if (enemyStats.health <= 0 || agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        //{
        //    return;
        //}

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
            //Debug.LogWarning($"{gameObject.name} is off NavMesh! Trying to reposition...");
            RepositionToNavMesh();
        }
    }

    void RepositionToNavMesh()
    {
        NavMeshHit hit;
        float searchRadius = 10f; // Bán kính tìm vị trí hợp lệ

        if (NavMesh.SamplePosition(transform.position, out hit, searchRadius, NavMesh.AllAreas))
        {
            transform.position = hit.position; // Dịch Enemy về vị trí hợp lệ
            agent.Warp(hit.position); // Đưa AI trở lại NavMesh
            //Debug.Log($"{gameObject.name} repositioned to valid NavMesh area.");
        }
        else
        {
            Debug.LogError($"{gameObject.name} could not find a valid NavMesh position! Moving to spawn point.");
            MoveToSpawnPoint();
        }
    }

    void MoveToSpawnPoint()
    {
        Transform spawnPoint = FindNearestSpawnPoint();
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            agent.Warp(spawnPoint.position);
            Debug.Log($"{gameObject.name} moved to nearest spawn point.");
        }
        else
        {
            Debug.LogError($"No valid spawn points found for {gameObject.name}!");
        }
    }

    Transform FindNearestSpawnPoint()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject spawn in spawnPoints)
        {
            float distance = Vector3.Distance(transform.position, spawn.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = spawn.transform;
            }
        }

        return closestPoint;
    }
}
