using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyClimb : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;
    private bool isClimbing = false;
    private bool MoveAcrossNavMeshesStarted = false;

    public float climbSpeed = 3f; // Tốc độ leo
    public float detectWallDistance = 1.5f; // Khoảng cách phát hiện tường
    public float extraClimbHeight = 0.4f; // Độ cao leo thêm
    public float stepForwardDistance = 0.5f; // Tiến lên một chút sau khi leo
    public LayerMask climbableLayer; // Layer của tường

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!isClimbing)
        {
            DetectWallAndClimb();
        }

        if (agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted)
        {
            StartCoroutine(MoveAcrossNavMeshLink());
            MoveAcrossNavMeshesStarted = true;
        }
    }

    void DetectWallAndClimb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectWallDistance, climbableLayer))
        {
            StartCoroutine(ClimbWall(hit));
        }
    }

    IEnumerator ClimbWall(RaycastHit hit)
    {
        isClimbing = true;
        agent.enabled = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        animator.Play("Climb"); // Đổi animation nếu có

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, hit.point.y + extraClimbHeight, startPosition.z);

        float elapsedTime = 0f;
        float duration = (targetPosition.y - startPosition.y) / climbSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        StartCoroutine(JumpDown());
    }

    IEnumerator JumpDown()
    {
        rb.useGravity = true;
        yield return new WaitForSeconds(1f);

        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
        agent.enabled = true;
        isClimbing = false;
    }

    IEnumerator MoveAcrossNavMeshLink()
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        agent.updateRotation = false;

        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float duration = (endPos - startPos).magnitude / agent.velocity.magnitude;
        float t = 0.0f;
        float tStep = 1.0f / duration;

        while (t < 1.0f)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            agent.destination = transform.position; // Cập nhật destination liên tục
            t += tStep * Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        agent.updateRotation = true;
        agent.CompleteOffMeshLink();
        MoveAcrossNavMeshesStarted = false;

        // Cập nhật điểm đến sau khi hoàn tất OffMesh Link
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
    }
}
