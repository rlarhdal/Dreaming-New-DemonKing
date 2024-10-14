using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int power;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �±װ� �÷��̾��̸鼭 IDamagable�� ���θ� Ȯ��
        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            // �������� �԰� ��
            damagable.ApplyDamage(power);
        }
    }
}
