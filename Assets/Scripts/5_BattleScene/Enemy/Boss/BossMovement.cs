using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private BossEnemy enemy;
    public Rigidbody2D rigid;
    
    private float tolerance = 1f; // �Ÿ� �Ӱ谪
    private Vector3 direction;    // �Ÿ�
    private Vector3 targetPos;    // Ÿ���� ��ġ
    private float speed = 0;      // ������ ������ �ӵ�

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

    // ������ ������ ����� ���� RigidBody�� ������ �����ǰ� ����
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

    // �÷��̾��� �ӵ� ���� �ʱ�ȭ => Ÿ���� ���� �� �ְ� ��
    public void Rush()
    {
        speed = 30.0f;
    }

    // Ÿ�� ����
    public void TargetLock()
    {
        targetPos = enemy.target.position;
    }

    // �Ű����� target�� ���� speed��ŭ �뽬
    public void AddForce(Vector3 target, float speed)
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(target.normalized * speed, ForceMode2D.Impulse);
    }
}
