using UnityEngine;

public enum AnimationState
{
    Idle,
    Move
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick _playerJoystick;
    [SerializeField] private float _moveSpeed;

    //private CharacterController controller;
    private Rigidbody rb;
    private Animator animator;
    private AnimationState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Tìm Animator trong Object con
    }

    void Update()
    {
        // Lấy giá trị từ joystick (chỉ dùng X, Z vì game 3D)
        Vector2 input = _playerJoystick.GetMoveVector();
        Vector3 move = new Vector3(input.x, 0, input.y) * _moveSpeed * Time.deltaTime;

        //// Di chuyển nhân vật
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        // **Kiểm tra tốc độ để cập nhật Animation**
        if (move.magnitude > 0.1f) // Nếu có di chuyển
        {
            ChangeAnimationState(AnimationState.Move);
        }
        else // Nếu dừng lại
        {
            ChangeAnimationState(AnimationState.Idle);
        }
    }

    void ChangeAnimationState(AnimationState newState)
    {
        if (currentState == newState) return;

        if (animator != null) // Kiểm tra nếu Animator đã được tìm thấy
        {
            animator.Play(newState.ToString()); // Đặt Animation theo tên Enum
            currentState = newState;
        }
        else
        {
            Debug.LogError("Animator không được tìm thấy! Hãy kiểm tra lại cấu trúc Hierarchy.");
        }
    }
}
