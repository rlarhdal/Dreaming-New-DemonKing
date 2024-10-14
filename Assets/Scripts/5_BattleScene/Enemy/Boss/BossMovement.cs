using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private BossEnemy enemy;
    public Rigidbody2D rigid;
    
    private float tolerance = 1f; // 거리 임계값
    private Vector3 direction;    // 거리
    private Vector3 targetPos;    // 타겟의 위치
    private float speed = 0;      // 보스의 움직임 속도

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        enemy = GetComponent<BossEnemy>();
    }

    private void FixedUpdate()
    {
        if (speed > 0)
        {
            direction = targetPos - transform.position;
            if (direction.magnitude > tolerance)
            {
                rigid.velocity = direction.normalized * speed;
            }
            else
            {

                rigid.velocity = Vector3.zero;
                speed = 0;
            }
        }
    }

    // 보스의 패턴을 사용할 때만 RigidBody의 고정을 해제되게 설정
    public void MoveChange()
    {
        if (rigid.constraints == RigidbodyConstraints2D.FreezeRotation)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    // 플레이어의 속도 값을 초기화 => 타겟을 쫓을 수 있게 됨
    public void Rush()
    {
        speed = 30.0f;
    }

    // 타겟 고정
    public void TargetLock()
    {
        targetPos = enemy.target.position;
    }

    // 매개변수 target을 향해 speed만큼 대쉬
    public void AddForce(Vector3 target, float speed)
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(target.normalized * speed, ForceMode2D.Impulse);
    }
}
