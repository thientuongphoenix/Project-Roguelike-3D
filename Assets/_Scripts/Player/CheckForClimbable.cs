using UnityEngine;

public class CheckForClimbable : MonoBehaviour
{
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _climbableLayer;
    [SerializeField] private Transform _model; // Gán Model trong Inspector
    private SphereCollider _playerCollider;

    void Start()
    {
        _playerCollider = GetComponent<SphereCollider>(); // Lấy Collider của nhân vật
    }

    private void Update()
    {
        CheckClimbable();
    }

    private void CheckClimbable()
    {
        // Lấy vị trí và hướng của nhân vật
        Vector3 startPosition = _playerCollider.bounds.center; // Tâm của Collider
        Vector3 direction = _model.forward; // Dùng forward của Model thay vì Player

        RaycastHit hit;

        if (Physics.Raycast(startPosition, direction, out hit, _rayDistance, _climbableLayer))
        {
            Debug.Log("Phát hiện vật thể phía trước: " + hit.collider.name);
            Debug.DrawRay(startPosition, direction * hit.distance, Color.red); // Vẽ ray khi có va chạm
        }
        else
        {
            Debug.DrawRay(startPosition, direction * _rayDistance, Color.green); // Vẽ ray không có va chạm
        }
    }
}
