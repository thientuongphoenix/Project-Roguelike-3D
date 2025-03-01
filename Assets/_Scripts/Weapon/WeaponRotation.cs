using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    private WeaponDetection detectionSystem; // Hệ thống phát hiện kẻ địch
    public float rotationSpeed = 10f; // Tốc độ xoay

    private void Start()
    {
        // Lấy reference tới WeaponDetection
        detectionSystem = GetComponent<WeaponDetection>();

        if (detectionSystem == null)
        {
            Debug.LogError("WeaponRotation: Không tìm thấy WeaponDetection trên cha!");
        }
    }

    private void Update()
    {
        if (detectionSystem == null) return;

        Transform target = detectionSystem.GetClosestEnemy();
        if (target != null)
        {
            RotateToTarget(target);
        }
    }

    void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Giữ súng không bị chúi xuống đất

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Nếu súng cần xoay thêm góc (tùy vào trục của prefab), chỉnh Euler tại đây
        lookRotation *= Quaternion.Euler(-90, 0, 0); // Điều chỉnh nếu cần

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
