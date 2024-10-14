using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : NormalEnemy
{
    [SerializeField]
    private GameObject attackRange;

    // ���� ������ ��Ȱ��ȭ
    private void AttackRangeActive()
    {
        attackRange.SetActive(false);
    }

    // ���� ���� ������Ʈ�� Ȱ��ȭ�ϰ� ���� ���� �Ѱ���
    private void AttackStart()
    {
        clips.PlaySFX(EnemyClip.Attack);
        attackRange.SetActive(true);
        attackRange.GetComponent<MeleeAttack>().power = status.attack;
    }
}
