using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyClimbMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isClimbing = false;

    public AnimationCurve climbCurve; // Curve trong Animation làm mượt tốc độ leo

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClimbPoint") && !isClimbing)
        {
            StartCoroutine(SmoothClimb(other.transform.position));
        }
    }

    IEnumerator SmoothClimb(Vector3 targetPosition)
    {
        isClimbing = true;
        agent.enabled = false;

        Vector3 startPosition = transform.position;
        float duration = 1.5f; // Thời gian leo lên (giảm nếu muốn nhanh hơn)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, climbCurve.Evaluate(t));
            yield return null;
        }

        agent.enabled = true;
        isClimbing = false;
    }
}
