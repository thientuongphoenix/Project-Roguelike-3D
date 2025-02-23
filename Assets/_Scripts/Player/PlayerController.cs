using System.Collections;
using Unity.Android.Gradle.Manifest;
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
    [SerializeField] private GameObject _model; // Dùng để xoay _model theo hướng di chuyển

    [SerializeField] private float _climbSpeed; // Tốc độ leo lên
    [SerializeField] private CheckForClimbable _checkForClimbable;

    private Rigidbody _rb;
    private Animator _animator;
    private bool _isClimbing;
    private AnimationState _currentState; // Chuyển State Animation bằng String

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _checkForClimbable = GetComponent<CheckForClimbable>();
        _animator = GetComponentInChildren<Animator>(); // Tìm Animator trong Object con
    }

    void Update()
    {
        if (_checkForClimbable._canClimb && !_isClimbing)
        {
            StartCoroutine(ClimbUp());
        }
        else
        {
            PlayerMove();
        }
    }

    void PlayerMove()
    {
        // Lấy giá trị từ joystick (chỉ dùng X, Z vì game 3D)
        Vector2 input = _playerJoystick.GetMoveVector();
        Vector3 move = new Vector3(input.x, 0, input.y) * _moveSpeed * Time.deltaTime;

        // Di chuyển nhân vật
        _rb.linearVelocity = new Vector3(move.x, _rb.linearVelocity.y, move.z);

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

    IEnumerator ClimbUp()
    {
        _isClimbing = true;
        _rb.useGravity = false; //Tắt trọng lực
        ChangeAnimationState(AnimationState.Climb);

        float extraClimbHeight = 0.3f;
        RaycastHit hit;

        //tiếp tục leo nếu Raycast còn check được Layer Climbable
        while (Physics.Raycast(transform.position, transform.forward, out hit, _checkForClimbable._rayDistance, _checkForClimbable._climbableLayer))
        {
            transform.position += Vector3.up * _climbSpeed * Time.deltaTime;
            yield return null;
        }

        // Khi không còn Climbable, leo thêm một đoạn nhỏ
        float finalHeight = transform.position.y + extraClimbHeight;
        while (transform.position.y < finalHeight)
        {
            transform.position += Vector3.up * _climbSpeed * Time.deltaTime;
            yield return null;
        }

        _rb.useGravity = true; // Bật lại trọng lực sau khi leo lên
        _isClimbing = false;
        ChangeAnimationState(AnimationState.Idle);
    }

    void ChangeAnimationState(AnimationState newState)
    {
        if (_currentState == newState) return;

        if (_animator != null) // Kiểm tra nếu Animator đã được tìm thấy
        {
            _animator.Play(newState.ToString()); // Đặt Animation theo tên Enum
            _currentState = newState;
        }
        else
        {
            Debug.LogError("Animator không được tìm thấy! Hãy kiểm tra lại cấu trúc Hierarchy.");
        }
    }
}
