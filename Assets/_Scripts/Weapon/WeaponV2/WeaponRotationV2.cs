using UnityEngine;

/// <summary>
/// Hệ thống xoay vũ khí về phía enemy gần nhất.
/// </summary>
public class WeaponRotationV2 : MonoBehaviour
{
    public float rotationSpeed = 10f;
    private WeaponDetectionV2 detectionSystem;

    void Start()
    {
        detectionSystem = GetComponent<WeaponDetectionV2>();
    }

    void Update()
    {
        RotateToTarget();
    }

    /// <summary>
    /// Xoay vũ khí về phía mục tiêu.
    /// </summary>
    void RotateToTarget()
    {
        Transform target = detectionSystem.GetTarget();
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Ngăn súng bị chúi xuống đất

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Nếu cần chỉnh thêm góc, có thể thay đổi Euler angles tại đây
        lookRotation *= Quaternion.Euler(-90, 0, 0); // Điều chỉnh nếu cần

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
