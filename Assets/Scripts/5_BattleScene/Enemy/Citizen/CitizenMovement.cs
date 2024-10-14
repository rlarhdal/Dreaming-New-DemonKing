using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenMovement : MonoBehaviour
{
    private CitizenEnemy enemy;
    [SerializeField] private NavMeshAgent agent;

    private Vector3 flipX = new Vector3(1, 1, 1);
    public Vector3 runPos; // ���� ����
    private Vector3 wanderPos; // ��ȸ ����

    // �� �̵� ����
    bool turn = false;

    // ĳ��
    WaitForSeconds waitDalay = new WaitForSeconds(5.0f);

    // �ڽ��� ���� �ൿ ����
    private EnemyState currentState;

    private void Awake()
    {
        enemy = GetComponent<CitizenEnemy>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        enemy.hitEvent += StopMove;
        enemy.dieEvent += StopMove;

        agent.enabled = true;
        agent.speed = enemy.status.moveSpeed;
        StartCoroutine(SetWanderning());
    }

    private void OnEnable()
    {
        currentState = EnemyState.Idle;
    }

    private void FixedUpdate()
    {
        if (!agent.isOnNavMesh) return;

        if (enemy.status.health <= 0) return;

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
        currentState = EnemyState.Wander;
    }

    // ���¿� ���� �ൿ ����
    private void State()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;

            case EnemyState.Wander:
                Movement(wanderPos);
                break;

            case EnemyState.Turn:
                Movement(runPos);
                break;
        }
    }

    // Ÿ��(��Ż �Ǵ� ���� ���)���� �̵�
    private void Movement(Vector3 goalPos)
    {
        agent.SetDestination(goalPos);
        Rotate(goalPos - transform.position);
        // ���� Wander������ ��� Ÿ���� �Ÿ��� ��������� ��� ���·� ����
        if (currentState == EnemyState.Wander)
        {
            if((goalPos - transform.position).magnitude < 0.1f)
            {
                currentState = EnemyState.Idle;
                enemy.idleEvent?.Invoke();
            }
        }
    }

    // �ݺ������� ������
    IEnumerator SetWanderning()
    {
        NavMeshHit hit;

        while (!turn)
        {
            currentState = EnemyState.Wander;
            enemy.moveEvent?.Invoke();

            // �ڽ��� �߽����� 3~5ũ���� ���� ���� �ڸ��� �̵�
            wanderPos = transform.position + Random.insideUnitSphere * Random.Range(3, 5);

            for (int i = 0; i < 10; i++)
            {
                if (NavMesh.SamplePosition(wanderPos, out hit, 5.0f, NavMesh.AllAreas))
                {
                    wanderPos = hit.position;
                    break;
                }
                wanderPos = hit.position;
            }

            yield return waitDalay;
        }
    }

    // �̵��� �������� ���� ����
    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flipX.x = Mathf.Abs(rotZ) > 90f ? 1 : -1;
        transform.localScale = flipX;
    }

    // �÷��̾�� �浹�� �������� ���·� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentState = EnemyState.Turn;
            enemy.moveEvent?.Invoke();
            agent.isStopped = false;
        }
    }
}
