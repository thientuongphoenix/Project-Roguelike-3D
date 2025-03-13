using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; // Player để rượt đuổi
    public float moveSpeed = 3f; // Tốc độ di chuyển
    public float stoppingDistance = 1.5f; // Khoảng cách tối thiểu trước khi dừng lại

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Tìm Player
    }

    void Update()
    {
        if (player == null) return; // Không có Player thì không làm gì cả

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stoppingDistance) // Chỉ di chuyển khi chưa đến gần Player
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized; // Hướng đến Player
        rb.linearVelocity = direction * moveSpeed; // Di chuyển bằng Rigidbody
    }
}
