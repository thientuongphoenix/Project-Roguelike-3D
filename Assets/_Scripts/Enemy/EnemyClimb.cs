using UnityEngine;
using System.Collections;

using UnityEngine.AI;

public class EnemyClimb : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(Climb());
        }
    }

    IEnumerator Climb()
    {
        // Tạm dừng NavMeshAgent
        agent.isStopped = true;

        // Bắt đầu animation leo trèo
        animator.SetTrigger("Climb");

        // Thời gian leo trèo (tùy chỉnh theo animation)
        yield return new WaitForSeconds(1.5f);

        // Hoàn thành OffMeshLink
        agent.CompleteOffMeshLink();

        // Tiếp tục NavMeshAgent
        agent.isStopped = false;
    }
}
