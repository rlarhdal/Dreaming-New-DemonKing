using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAnimator : MonoBehaviour
{
    CitizenEnemy enemy;
    Animator animator;

    private void Awake()
    {
        enemy = GetComponent<CitizenEnemy>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enemy.dieEvent += Die;
        enemy.hitEvent += Hit;
        enemy.moveEvent += Move;
        enemy.idleEvent += Idle;
    }

    void Idle()
    {
        animator.Play("Idle");
    }

    void Move()
    {
        animator.Play("Move");
    }

    void Die()
    {
        animator.Play("Die");
    }

    void Hit()
    {
        animator.Play("Hit");
    }
}
