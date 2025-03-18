using UnityEngine;

public enum PlayerAnimationState
{
    Idle,
    Move,
    Climb,
    Jump,
    Fly,
    Die
}

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerAnimationState _currentState;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        if (_animator == null)
        {
            Debug.LogError("[ERROR] Không tìm thấy Animator trên " + gameObject.name);
        }
    }

    public void ChangeAnimationState(PlayerAnimationState newState)
    {
        if (_currentState == newState) return;

        if (_animator != null)
        {
            _animator.Play(newState.ToString()); // Chạy animation dựa vào Enum
            _currentState = newState;
        }
    }
}
