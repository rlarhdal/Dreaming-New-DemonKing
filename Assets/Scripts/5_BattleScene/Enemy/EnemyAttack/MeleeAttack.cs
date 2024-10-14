using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int power;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 태그가 플레이어이면서 IDamagable의 여부를 확인
        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            // 데미지를 입게 함
            damagable.ApplyDamage(power);
        }
    }
}
