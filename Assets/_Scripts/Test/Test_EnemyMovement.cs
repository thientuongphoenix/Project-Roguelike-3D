using UnityEngine;

public class Test_EnemyMovement : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        if(player == null)
        {
            Debug.LogWarning("Không tìm thấy PLayer!");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        Vector3 targetPosition = (Vector3) transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
    }
}
