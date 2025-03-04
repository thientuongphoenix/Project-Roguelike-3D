using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyClimb : MonoBehaviour
{
    [SerializeField] private float _rayDistance = 1.5f; // Khoảng cách phát hiện leo
    [SerializeField] private float _dropRayDistance = 2f; // Khoảng cách phát hiện vực
    [SerializeField] private LayerMask _climbableLayer; // Layer của block có thể leo
    [SerializeField] private LayerMask _groundLayer; // Layer của mặt đất
    [SerializeField] private Transform _model; // Transform của mô hình Enemy

    private NavMeshAgent agent;
    private bool _canClimb;
    private bool _canDrop;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckClimbable();
        CheckForDrop();

        if (_canClimb)
        {
            StartCoroutine(Climb());
        }
        else if (_canDrop)
        {
            StartCoroutine(Drop());
        }
    }

    /// <summary>
    /// Kiểm tra xem Enemy có thể leo lên block phía trước không
    /// </summary>
    private void CheckClimbable()
    {
        Vector3 startPosition = transform.position + Vector3.up * 0.5f; // Bắt đầu từ giữa Enemy
        Vector3 direction = _model.forward; // Hướng về phía trước

        RaycastHit hit;
        if (Physics.Raycast(startPosition, direction, out hit, _rayDistance, _climbableLayer))
        {
            Debug.DrawRay(startPosition, direction * hit.distance, Color.red); // Vẽ ray khi có va chạm
            _canClimb = true; // Enemy có thể leo
        }
        else
        {
            Debug.DrawRay(startPosition, direction * _rayDistance, Color.green); // Vẽ ray khi không có va chạm
            _canClimb = false; // Không thể leo
        }
    }

    /// <summary>
    /// Kiểm tra nếu phía trước Enemy không có mặt đất, nghĩa là có vực
    /// </summary>
    private void CheckForDrop()
    {
        Vector3 startPosition = transform.position + Vector3.up * 0.5f;
        Vector3 forwardCheck = transform.position + _model.forward * 1f; // Kiểm tra trước mặt

        RaycastHit hit;
        bool hasGround = Physics.Raycast(forwardCheck, Vector3.down, out hit, _dropRayDistance, _groundLayer);

        Debug.DrawRay(forwardCheck, Vector3.down * _dropRayDistance, hasGround ? Color.green : Color.yellow);

        if (!hasGround)
        {
            Debug.Log("Vực phía trước! Enemy chuẩn bị nhảy xuống.");
            _canDrop = true;
        }
        else
        {
            Debug.Log("Có mặt đất phía trước: " + hit.collider.name);
            _canDrop = false;
        }
    }

    /// <summary>
    /// Enemy leo lên block mà không cần animation
    /// </summary>
    IEnumerator Climb()
    {
        _canClimb = false;
        agent.isStopped = true;
        agent.enabled = false;

        Vector3 climbTarget = transform.position + Vector3.up * 2f; // Điểm leo lên
        float climbTime = 1f; // Thời gian leo
        float elapsedTime = 0f;

        while (elapsedTime < climbTime)
        {
            transform.position = Vector3.Lerp(transform.position, climbTarget, elapsedTime / climbTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = climbTarget; // Đảm bảo vị trí cuối cùng chính xác
        agent.enabled = true;
        agent.isStopped = false;
    }

    /// <summary>
    /// Enemy nhảy xuống vực mà không cần animation
    /// </summary>
    IEnumerator Drop()
    {
        _canDrop = false;
        agent.isStopped = true;
        agent.enabled = false;

        Vector3 dropTarget = transform.position + Vector3.down * 2f; // Điểm nhảy xuống
        RaycastHit hit;

        // Kiểm tra mặt đất bên dưới để tránh rơi mãi mãi
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _dropRayDistance, _groundLayer))
        {
            dropTarget = hit.point; // Đặt vị trí chính xác trên mặt đất
        }

        float dropTime = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < dropTime)
        {
            transform.position = Vector3.Lerp(transform.position, dropTarget, elapsedTime / dropTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = dropTarget; // Đảm bảo Enemy hạ cánh chính xác

        yield return new WaitForSeconds(0.2f); // Chờ để ổn định vị trí

        agent.enabled = true;
        agent.isStopped = false;
    }
}
