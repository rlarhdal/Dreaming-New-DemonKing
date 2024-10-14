using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    public BossEnemy enemy;
    Animator animator;

    // 보스의 애니메이션 변수
    private static readonly string die = "Die";
    private static readonly string stun = "Stun";
    private static readonly string attack1 = "Attack1";
    private static readonly string attack2 = "Attack2";
    private static readonly string attack3 = "Attack3";
    private static readonly string attack4 = "Attack4";
    private static readonly string special1 = "Special1";
    private static readonly string special2 = "Special2";

    private void Awake()
    {
        enemy = GetComponent<BossEnemy>();
        animator = GetComponent<Animator>();

        enemy.attack1Event += Attack1;
        enemy.attack2Event += Attack2;
        enemy.attack3Event += Attack3;
        enemy.attack4Event += Attack4;
        enemy.special1Event += Special1;
        enemy.special2Event += Special2;
        enemy.stunEvent += Stun;
        enemy.dieEvent += Die;
    }

    void Die()
    {
        animator.Play(die);
    }

    void Attack1()
    {
        animator.Play(attack1);
    }

    void Attack2()
    {
        animator.Play(attack2);
    }

    void Attack3()
    {
        animator.Play(attack3);
    }

    void Attack4()
    {
        animator.Play(attack4);
    }

    void Special1()
    {
        animator.Play(special1);
    }

    void Special2()
    {
        animator.Play(special2);
    }

    void Stun()
    {
        animator.Play(stun);
    }
}
