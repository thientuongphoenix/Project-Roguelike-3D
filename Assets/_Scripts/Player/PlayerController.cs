﻿using System.Collections;
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

    [SerializeField] private float climbSpeed; // Tốc độ leo lên
    [SerializeField] private float jumpForce; // Lực nhảy

    private Rigidbody rb;
    private Animator animator;
    private bool isClimbing;
    private bool isJumping;
    private bool isGrounded;
    private bool isFalling;
    private AnimationState currentState; // Chuyển State Animation bằng String

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Tìm Animator trong Object con
    }

    void Update()
    {
        PlayerMove();
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
