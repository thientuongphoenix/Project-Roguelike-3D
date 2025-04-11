using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform playerTransform; // biến lưu trữ vị trí của Player
    public float health = 100f; // biến lưu trữ máu của Boss
    public NavMeshAgent agent; // biến lưu trữ NavMeshAgent
    public int numberOfSummons = 5; // biến lưu trữ số lượng Summon
    bool isVisible = false; // biến kiểm tra trạng thái của Boss

    public Transform spawnPoint; //Chỗ gọi đệ
    public GameObject spawnPrefab; //Prefab đệ Boss

    Node rootNode; // biến lưu trữ node gốc của cây hành vi
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rootNode = new Selector(new List<Node>()
        {

        });
    }


    // Update is called once per frame
    void Update()
    {
        if (isVisible == false) return;
        rootNode.Evaluate(); // đánh giá trạng thái của cây hành vi
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}

//Kiểm tra máu để quyết định là có chạy trốn hay không ?
public class CheckFleeHealthNode : Node
{
    private Boss boss;

    public CheckFleeHealthNode(Boss boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        if(boss.health <= 30f)
        {
            Debug.Log("Boss đang dưới 30% máu");
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}

//Chạy trốn, chạy ngược hướng với Player
public class FleeNode : Node
{
    private Boss boss;
    private Transform bossTransform;
    private Transform playerTransform;

    private float fleeDistance = 10f;

    public FleeNode(Boss boss, Transform bossTransform, Transform playerTransform)
    {
        this.boss = boss;
        this.bossTransform = boss.transform;
        this.playerTransform = playerTransform;
    }

    public override NodeState Evaluate()
    {
        //Tính hướng chạy ngược lại
        Vector3 directionAway = (bossTransform.position - playerTransform.position).normalized;
        Vector3 fleeTarget = bossTransform.position + (directionAway * fleeDistance);
        
        //Di chuyển tới cái hướng đó
        boss.agent.SetDestination(fleeTarget);
        if (Vector3.Distance(bossTransform.position, fleeTarget) >= 0.1f)
        {
            Debug.Log("Boss đang trên đường chạy trốn");
            return NodeState.RUNNING;
        }
        else if (Vector3.Distance(bossTransform.position, fleeTarget) <= 0.1f)
        {
            Debug.Log("Boss đang chạy trốn");
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}

//Kiểm tra đk để triệu hồi đệ
public class CheckSummonConditionNode : Node
{
    private Boss boss;

    public CheckSummonConditionNode(Boss boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        if(boss.health <= 50f)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}

public class SummonNode : Node
{
    private Boss boss;

    private float coolDown = 10f;

    public SummonNode(Boss boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        if (coolDown <= 0)
        {
            Debug.Log("Boss dưới 50% máu, dùng chiêu gọi đệ");
            //Instantiate(boss.spawnPrefab, boss.spawnPoint.position, Quaternion.identity); 
            //Viết class ở ngoài rồi gọi vào
            coolDown = 10;
            return NodeState.SUCCESS;
        }
        else
        {
            Debug.Log("Chiêu gọi đệ đang hồi");
            coolDown -= Time.deltaTime;
            return NodeState.FAILURE;
        }
    }
}

//Kiểm tra đk Buff
public class CheckBuffCondition : Node
{
    private Boss boss;

    public CheckBuffCondition(Boss boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        if(boss.health == 70 || boss.health == 40)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}

//Buff cho chính nó
public class SelfBuffNode : Node
{
    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
        //tăng damage
        //tăng tốc độ di chuyển
        //cooldown
    }
}

// Kiểm tra điều kiện tấn công
public class CheckAttackConditionNode : Node
{
    public override NodeState Evaluate()
    {
        // khoảng cách giữa Boss và Player
        // cooldown tấn công
        throw new System.NotImplementedException();
    }
}

// Tấn công Player
public class AttackNode : Node
{
    public override NodeState Evaluate()
    {
        // tấn công Player
        // cooldown tấn công
        throw new System.NotImplementedException();
    }
}

// Kiểm tra điều kiện đi tuần
public class CheckPatrolConditionNode : Node
{
    public override NodeState Evaluate()
    {
        // khoảng cách giữa Boss và Player
        // cooldown đi tuần
        throw new System.NotImplementedException();
    }
}

// Đi tuần
public class PatrolNode : Node
{
    public override NodeState Evaluate()
    {
        // đi tuần
        // cooldown đi tuần
        throw new System.NotImplementedException();
    }
}


