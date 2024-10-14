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
    private float tolerance = 0.1f; // 거리 임계값
    public Vector3 turnPos; // 시작 지점

    // 플레이어 추격 관련
    public bool attacking = false;
    public float trackingTime = 0.0f;

    // 현재 상태를 저장
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

    // 현재 상태의 값을 변경
    public void StateChange(EnemyState state)
    {
        currentState = state;
    }

    // 현재 상태의 값을 불러온다.
    public EnemyState GetState()
    {
        return currentState;
    }

    // 상태에 따른 행동
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
        // 베이크된 곳이 아니라면 리턴
        if (!agent.isOnNavMesh) return;

        // 체력이 0이라면 속도를 멈춤
        if (enemy.status.health <= 0)
        {
            agent.speed = 0;
            return;
        }
        
        // 추격시간이 존재하면 추격시간이 감소되고
        // 공격중일 때는 추격시간이 감소되지 않게 함
        if (trackingTime > 0)
        {
            if (!attacking)
            {
                trackingTime -= Time.fixedDeltaTime;
            }
        }
        else
        {
            // 만약 추격시간이 끝났다면 적의 상태를 Turn으로 설정
            // 원래 자신의 자리로 이동되게 함
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

    // 추격도중 공격거리가 플레이어와의 거리보다 커진다면
    // 플레이어를 공격하고 공격여부가 아니라면 대기를 하게 한다.
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

    // 타겟을 향해서 이동을 하고 Chase와 Turn에 따라서 대기 또는 공격 상태로 변경되게 한다.
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

    // direction 방향에 따른 좌우 반전
    protected void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flipX.x = Mathf.Abs(rotZ) > 90f ? 1 : -1;
        transform.localScale = flipX;
    }

    // 플레이어의 거리가 가까워지면 플레이어를 추격
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
