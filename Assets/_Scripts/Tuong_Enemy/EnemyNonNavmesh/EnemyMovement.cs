using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // Player để rượt đuổi
    public float moveSpeed = 3f; // Tốc độ di chuyển
    public float stoppingDistance = 1.5f; // Khoảng cách tối thiểu trước khi dừng lại

    [Header("Climbing Settings")]
    public Transform raycastOrigin; // Điểm xuất phát của Raycast
    public float raycastMaxDistance;
    public LayerMask climbableLayer; // Layer của vật thể có thể leo
    public float climbSpeed = 2.0f; // Tốc độ leo lên
    public float stepUpHeight = 1f; // 🔥 Sau khi leo xong, nâng lên thêm 1f
    public float stepForwardDistance = 1f; // 🔥 Bước tới trước sau khi leo

    private Rigidbody rb;
    private bool isClimbing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Tìm Player
    }

    void Update()
    {
        if (player == null) return; // Không có Player thì không làm gì cả

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isClimbing) // Chỉ di chuyển nếu không leo
        {
            if (distance > stoppingDistance)
            {
                MoveTowardsPlayer();
            }

            CheckForClimbable(); // Kiểm tra xem có thể leo không
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized; // Hướng về Player
        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed); // Giữ lại trục Y
    }

    void CheckForClimbable()
    {
        if (player == null) return;

        RaycastHit hit;
        Vector3 startPosition = raycastOrigin.position;

        // 🔥 Tạo hướng raycast theo mặt phẳng X-Z (Không chỉa lên/xuống)
        Vector3 directionToPlayer = player.position - startPosition;
        directionToPlayer.y = 0; // 🔥 Loại bỏ Y để chỉ xoay quanh thân
        Vector3 direction = directionToPlayer.normalized; // Chuẩn hóa vector

        if (Physics.Raycast(startPosition, direction, out hit, raycastMaxDistance, climbableLayer))
        {
            Debug.DrawRay(startPosition, direction * hit.distance, Color.red, 0.1f); // 🔥 Raycast đỏ nếu trúng vật thể
            StartCoroutine(ClimbUp());
        }
        else
        {
            Debug.DrawRay(startPosition, direction * raycastMaxDistance, Color.green, 0.1f); // ✅ Raycast xanh nếu không trúng gì
        }
    }

    IEnumerator ClimbUp()
    {
        isClimbing = true;
        rb.useGravity = false; // 🔥 Tắt trọng lực
        rb.linearVelocity = Vector3.zero; // Dừng mọi di chuyển

        Debug.Log("Enemy bắt đầu leo!");

        RaycastHit hit;
        while (Physics.Raycast(raycastOrigin.position, (player.position - raycastOrigin.position).normalized, out hit, 1f, climbableLayer))
        {
            transform.position += Vector3.up * climbSpeed * Time.deltaTime;
            yield return null;
        }

        Debug.Log("Enemy đã leo hết vật thể!");

        // 🔥 Sau khi leo xong, nâng Enemy lên thêm 1 đơn vị để tránh bị kẹt
        float targetHeight = transform.position.y + stepUpHeight;
        while (transform.position.y < targetHeight)
        {
            transform.position += Vector3.up * climbSpeed * Time.deltaTime;
            yield return null;
        }

        // 🔥 Tiến lên phía trước 1 khoảng để tránh bị dính vào mép vật thể
        Vector3 stepForward = (player.position - transform.position).normalized * stepForwardDistance;
        Vector3 targetPosition = transform.position + new Vector3(stepForward.x, 0, stepForward.z);
        float moveSpeed = 2f; // Tốc độ bước tới

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Enemy đã bước lên thành công!");

        rb.useGravity = true; // 🔥 Bật lại trọng lực
        isClimbing = false; // Cho phép tiếp tục đuổi theo Player

        Debug.Log("Enemy tiếp tục rượt theo Player!");
    }

    //void OnDrawGizmos()
    //{
    //    if (raycastOrigin != null && player != null)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Vector3 direction = (player.position - raycastOrigin.position).normalized;
    //        Gizmos.DrawLine(raycastOrigin.position, raycastOrigin.position + direction * raycastMaxDistance);
    //    }
    //}
}
