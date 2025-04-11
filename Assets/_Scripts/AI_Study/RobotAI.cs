using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour
{
    public NavMeshAgent agent; // biến lưu trữ NavMeshAgent
    public Transform playerTransform; // biến lưu trữ vị trí của Player

    private Node rootNode; // biến lưu trữ node gốc của cây hành vi

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rootNode = new Selector(new List<Node>()
        {
            new Sequence(new List<Node>()
            {
                new CheckPlayerDistance(playerTransform, transform),
                new AttackPlayer(playerTransform, transform),
            }),
            new Sequence(new List<Node>()
            {
                new CheckPlayerDistance(playerTransform, transform),
                new ChasePlayer(playerTransform, transform, this)
            })
        });
    }

    // Update is called once per frame
    void Update()
    {
        rootNode.Evaluate(); // đánh giá trạng thái của cây hành vi
    }

    public void ChasePlayerTarget()
    {
        // thực hiện hành động đuổi theo Player
        Debug.Log("Chase Player Target");
    }
}

// Kiểm tra khoảng cách giữa robot và mục tiêu Player
public class CheckPlayerDistance : Node
{
    private Transform playerTransform; // biến lưu trữ vị trí của Player
    private Transform robotTransform; // biến lưu trữ vị trí của Robot

    private float distanceRange = 5f; // khoảng cách tối đa giữa Robot và Player

    public CheckPlayerDistance(Transform playerTransform,
        Transform robotTransform)
    {
        this.playerTransform = playerTransform; // gán vị trí của Player
        this.robotTransform = robotTransform; // gán vị trí của Robot
    }

    public override NodeState Evaluate()
    {
        var distance = Vector3.Distance(playerTransform.position,
            robotTransform.position); // tính khoảng cách giữa Robot và Player
        Debug.Log("....CheckPlayerDistance: " + distance); // in ra thông báo kiểm tra khoảng cách
        if (distance < distanceRange) // nếu khoảng cách nhỏ hơn khoảng cách tối đa
        {
            state = NodeState.SUCCESS;
            return state; // trả về trạng thái thành công
        }
        state = NodeState.FAILURE; // gán trạng thái là thất bại
        return state; // trả về trạng thái thất bại
    }
}

// Đuổi theo Player
public class ChasePlayer : Node
{
    private Transform playerTransform; // biến lưu trữ vị trí của Player
    private Transform robotTransform; // biến lưu trữ vị trí của Robot
    // private NavMeshAgent agent; // biến lưu trữ NavMeshAgent
    private RobotAI robotAI; // biến lưu trữ RobotAI
    public ChasePlayer(Transform playerTransform,
        Transform robotTransform, RobotAI robotAI)
    {
        this.playerTransform = playerTransform; // gán vị trí của Player
        this.robotTransform = robotTransform; // gán vị trí của Robot
        this.robotAI = robotAI; // gán NavMeshAgent
    }

    public override NodeState Evaluate()
    {
        Debug.Log("....ChasePlayer: " + playerTransform.position); // in ra thông báo đuổi theo Player
        // agent.SetDestination(playerTransform.position); 
        robotAI.ChasePlayerTarget();
        // gán vị trí đích cho NavMeshAgent

        if (robotAI.agent.remainingDistance < robotAI.agent.stoppingDistance) // nếu khoảng cách còn lại nhỏ hơn khoảng cách dừng
        {
            state = NodeState.SUCCESS; // gán trạng thái là thành công
            return state; // trả về trạng thái thành công
        }
        state = NodeState.RUNNING; // gán trạng thái là đang chạy
        return state; // trả về trạng thái đang chạy
    }
}

// Tấn công
public class AttackPlayer : Node
{
    private float attackRange = 1f; // khoảng cách tấn công
    private float damage = 10f; // sát thương
    private Transform playerTransform; // biến lưu trữ vị trí của Player
    private Transform robotTransform; // biến lưu trữ vị trí của Robot

    public AttackPlayer(Transform playerTransform,
        Transform robotTransform)
    {
        this.playerTransform = playerTransform; // gán vị trí của Player
        this.robotTransform = robotTransform; // gán vị trí của Robot
    }

    public override NodeState Evaluate()
    {
        var distance = Vector3.Distance(playerTransform.position,
            robotTransform.position); // tính khoảng cách giữa Robot và Player
        Debug.Log("....AttackPlayer: " + distance); // in ra thông báo tấn công Player
        if (distance < attackRange) // nếu khoảng cách nhỏ hơn khoảng cách tấn công
        {
            // thực hiện tấn công Player
            Debug.Log("Attack Player with damage: " + damage); // in ra thông báo tấn công Player
            state = NodeState.SUCCESS; // gán trạng thái là thành công
            return state; // trả về trạng thái thành công
        }
        state = NodeState.FAILURE; // gán trạng thái là thất bại
        return state; // trả về trạng thái thất bại
    }
}
