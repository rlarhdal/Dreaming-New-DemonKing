using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : NormalEnemy
{
    [SerializeField]
    private GameObject attackRange;

    // 공격 범위를 비활성화
    private void AttackRangeActive()
    {
        attackRange.SetActive(false);
    }

    // 공격 범위 오브젝트를 활성화하고 공격 값을 넘겨줌
    private void AttackStart()
    {
        clips.PlaySFX(EnemyClip.Attack);
        attackRange.SetActive(true);
        attackRange.GetComponent<MeleeAttack>().power = status.attack;
    }
}
