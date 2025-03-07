using UnityEngine;

public enum EnemyAnimationState
{
    Idle,
    Move,
    Attack,
    Climb,
    Die
}

public class EnemyAnimationController_Tuong : MonoBehaviour
{
    private Animator _animator;
    private EnemyAnimationState _currentState;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("[ERROR] Không tìm thấy Animator trên " + gameObject.name);
        }
    }

    public void ChangeAnimationState(EnemyAnimationState newState)
    {
        if (_currentState == newState) return;

        if (_animator != null)
        {
            _animator.Play(newState.ToString()); // Chạy animation dựa vào Enum
            _currentState = newState;
        }
    }
}
