using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick playerJoystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = 9.81f; // Trọng lực

    //private CharacterController controller;
    private Rigidbody rb;
    private Vector3 velocity; // Dùng để xử lý trọng lực

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Lấy giá trị từ joystick (chỉ dùng X, Z vì game 3D)
        Vector2 input = playerJoystick.GetMoveVector();
        Vector3 move = new Vector3(input.x, 0, input.y) * moveSpeed;

        //// trọng lực (nếu nhân vật không chạm đất)
        //if (!controller.isGrounded)
        //{
        //    velocity.y -= gravity * Time.deltaTime;
        //}
        //else
        //{
        //    velocity.y = -0.1f; // đặt giá trị nhỏ để giữ nhân vật chạm đất
        //}

        //// Di chuyển nhân vật
        //controller.Move(move * Time.deltaTime);
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
}
