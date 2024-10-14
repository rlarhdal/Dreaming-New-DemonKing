using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemyMovement : MonoBehaviour
{
    private NormalEnemy enemy;
    [SerializeField] private NavMeshAgent agent;

    private Vector3 flipX = new Vector3(1, 1, 1);
    private float tolerance = 0.1f; // �Ÿ� �Ӱ谪
    public Vector3 turnPos; // ���� ����

    // �÷��̾� �߰� ����
    public bool attacking = false;
    public float trackingTime = 0.0f;

    // ���� ���¸� ����
    private EnemyState currentState;

    private void Awake()
    {
        enemy = GetComponent<NormalEnemy>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemy.attackEvent += StopMove;
        enemy.hitEvent += StopMove;
        enemy.dieEvent += StopMove;
    }

    private void Start()
    {
        agent.enabled = true;
        agent.speed = enemy.status.moveSpeed;
    }

    private void OnEnable()
    {
        agent.speed = enemy.status.moveSpeed;
        currentState = EnemyState.Idle;
    }

    // ���� ������ ���� ����
    public void StateChange(EnemyState state)
    {
        currentState = state;
    }

    // ���� ������ ���� �ҷ��´�.
    public EnemyState GetState()
    {
        return currentState;
    }

    // ���¿� ���� �ൿ
    private void State()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;

            case EnemyState.Wait:
                WaitDistance();
                break;

            case EnemyState.Chase:
                Movement(enemy.target.position);
                break;

            case EnemyState.Turn:
                Movement(turnPos);
                break;

            case EnemyState.Attack:
                break;
        }
    }

    private void FixedUpdate()
    {
        // ����ũ�� ���� �ƴ϶�� ����
        if (!agent.isOnNavMesh) return;

        // ü���� 0�̶�� �ӵ��� ����
        if (enemy.status.health <= 0)
        {
            agent.speed = 0;
            return;
        }
        
        // �߰ݽð��� �����ϸ� �߰ݽð��� ���ҵǰ�
        // �������� ���� �߰ݽð��� ���ҵ��� �ʰ� ��
        if (trackingTime > 0)
        {
            if (!attacking)
            {
                trackingTime -= Time.fixedDeltaTime;
            }
        }
        else
        {
            // ���� �߰ݽð��� �����ٸ� ���� ���¸� Turn���� ����
            // ���� �ڽ��� �ڸ��� �̵��ǰ� ��
            if (currentState == EnemyState.Chase)
            {
                enemy.moveEvent?.Invoke();
                currentState = EnemyState.Turn;
            }
        }

        State();
    }

    private void StopMove()
    {
        agent.isStopped = true;
    }

    private void StartMove()
    {
        agent.isStopped = false;
        enemy.moveEvent?.Invoke();
        currentState = EnemyState.Chase;
    }

    // �߰ݵ��� ���ݰŸ��� �÷��̾���� �Ÿ����� Ŀ���ٸ�
    // �÷��̾ �����ϰ� ���ݿ��ΰ� �ƴ϶�� ��⸦ �ϰ� �Ѵ�.
    private void WaitDistance()
    {
        if ((enemy.target.position - transform.position).magnitude > enemy.status.attackRange)
        {
            enemy.moveEvent?.Invoke();
            StartMove();
            currentState = EnemyState.Chase;
        }
        else if (!attacking)
        {
            attacking = true;
            enemy.attackEvent?.Invoke();
            currentState = EnemyState.Attack;
        }
    }

    // Ÿ���� ���ؼ� �̵��� �ϰ� Chase�� Turn�� ���� ��� �Ǵ� ���� ���·� ����ǰ� �Ѵ�.
    private void Movement(Vector3 goalPos)
    {
        agent.SetDestination(goalPos);
        Rotate(goalPos - transform.position);
        if (currentState == EnemyState.Turn)
        {
            if ((goalPos - transform.position).magnitude < tolerance)
            {
                enemy.idleEvent?.Invoke();
                currentState = EnemyState.Idle;
            }
        }
        else if (currentState == EnemyState.Chase) 
        {
            if ((goalPos - transform.position).magnitude < enemy.status.attackRange)
            {
                if (!attacking)
                {
                    attacking = true;
                    enemy.attackEvent?.Invoke();
                    currentState = EnemyState.Attack;
                }
                else
                {
                    StopMove();
                    enemy.idleEvent?.Invoke();
                    currentState = EnemyState.Wait;
                }
            }
        }
    }

    // direction ���⿡ ���� �¿� ����
    protected void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flipX.x = Mathf.Abs(rotZ) > 90f ? 1 : -1;
        transform.localScale = flipX;
    }

    // �÷��̾��� �Ÿ��� ��������� �÷��̾ �߰�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentState == EnemyState.Idle)
            {
                currentState = EnemyState.Chase;
                enemy.moveEvent?.Invoke();
                trackingTime = 10.0f;
            }
        }
    }
}
