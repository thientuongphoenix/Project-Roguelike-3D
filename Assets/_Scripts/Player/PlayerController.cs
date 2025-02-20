using System.Collections;
using UnityEngine;

public enum AnimationState
{
    Idle,
    Move,
    Climb,
    Jump,
    Fly
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick _playerJoystick;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _model; // Dùng để xoay model theo hướng di chuyển

    [SerializeField] private LayerMask climbableLayer; // Layer của các vật có thể leo lên
    [SerializeField] private float climbHeight; // Chiều cao tối đa có thể leo
    [SerializeField] private float climbSpeed; // Tốc độ leo lên
    [SerializeField] private float jumpForce; // Lực nhảy

    private Rigidbody rb;
    private Animator animator;
    private bool isClimbing;
    private bool isJumping;
    private bool isGrounded;
    private AnimationState currentState; // Chuyển State Animation bằng String

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Tìm Animator trong Object con
    }

    void Update()
    {
        // Kiểm tra nếu nhân vật đang tiếp đất
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, LayerMask.GetMask("Ground"));

        if (!isClimbing) // Nếu không đang leo thì cho phép di chuyển
        {
            PlayerMove();
            CheckForClimbable();
        }

    }

    void PlayerMove()
    {
        // Lấy giá trị từ joystick (chỉ dùng X, Z vì game 3D)
        Vector2 input = _playerJoystick.GetMoveVector();
        Vector3 move = new Vector3(input.x, 0, input.y) * _moveSpeed * Time.deltaTime;

        // Di chuyển nhân vật
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        //xoay nhân vật theo hướng chuyển động
        if (move != Vector3.zero)
        {
            _model.transform.rotation = Quaternion.LookRotation(move * Time.deltaTime);
        }

        // Kiểm tra tốc độ để cập nhật Animation
        if (move.magnitude > 0.1f) // Nếu có di chuyển
        {
            ChangeAnimationState(AnimationState.Move);
        }
        else // Nếu dừng lại
        {
            ChangeAnimationState(AnimationState.Idle);
        }
    }

    void CheckForClimbable()
    {
        // Chỉ leo nếu nhân vật ĐANG ĐỨNG trên mặt đất
        if (!isGrounded) return;

        // Tạo một raycast phía trước nhân vật
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f; // Điểm bắn ray từ trước nhân vật
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(rayStart, rayDirection, out hit, 1f, climbableLayer))
        {
            float obstacleHeight = hit.point.y - transform.position.y;

            if (obstacleHeight > 0.3f && obstacleHeight <= climbHeight) // Chỉ leo khi chiều cao hợp lý
            {
                StartCoroutine(ClimbObstacle(hit.point.y));
            }
        }
    }

    IEnumerator ClimbObstacle(float targetY)
    {
        isClimbing = true;
        rb.linearVelocity = Vector3.zero; // Dừng di chuyển
        ChangeAnimationState(AnimationState.Climb);

        float startY = transform.position.y;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            float newY = Mathf.Lerp(startY, targetY + 1f, elapsedTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            elapsedTime += Time.deltaTime * climbSpeed;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY + 1f, transform.position.z);
        isClimbing = false;
        ChangeAnimationState(AnimationState.Idle);
    }

    void HandleJump()
    {
        // Nếu nhân vật đang trên mặt đất và nhấn phím SPACE thì nhảy
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            ChangeAnimationState(AnimationState.Jump);
        }

        // Khi nhân vật rơi xuống, chuyển sang trạng thái "Fly"
        if (rb.linearVelocity.y < -0.1f && !isGrounded)
        {
            ChangeAnimationState(AnimationState.Fly);
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
