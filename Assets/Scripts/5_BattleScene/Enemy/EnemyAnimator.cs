using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    public NormalEnemy enemy;
    Animator animator;

    private static readonly int isidle = Animator.StringToHash("IsIdle");
    private static readonly int isMove = Animator.StringToHash("IsMove");
    private static readonly int TriggerHit = Animator.StringToHash("Hit");
    private static readonly int TriggerDie = Animator.StringToHash("Die");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Awake()
    {
        enemy = GetComponent<NormalEnemy>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enemy.attackEvent += Attacking;
        enemy.hitEvent += Hit;
        enemy.moveEvent += Move;
        enemy.idleEvent += Idle;
        enemy.dieEvent += Die;
    }

    private void Idle()
    {
        animator.SetBool(isMove, false);
        animator.SetBool(isidle, true);
    }

    private void Move()
    {
        animator.SetBool(isidle, false);
        animator.SetBool(isMove, true);
    }

    private void Attacking()
    {
        animator.SetTrigger(Attack);
    }

    private void Hit()
    {
        animator.SetTrigger(TriggerHit);
    }

    private void Die()
    {
        animator.SetTrigger(TriggerDie);
    }
}
