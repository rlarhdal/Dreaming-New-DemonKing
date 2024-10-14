using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : NormalEnemy
{
    [SerializeField]
    private GameObject projectile;
    public bool AttackType = false;

    // ���Ÿ� ���� ���� ���� ����
    private void SpawnProjectile()
    {
        clips.PlaySFX(EnemyClip.Attack);
        // �߻����̶�� �÷��̾ ���ϰ� �ڽ��� ������ ���ݷ��� �������ش�.
        if(AttackType == false)
        {
            GameObject tile = Managers.Pool.Pop(projectile, transform.parent).gameObject;
            tile.transform.position = transform.position;
            tile.GetComponent<ProjectileController>().power = status.attack;
            tile.GetComponent<ProjectileController>().direction = (target.position - transform.position).normalized;
            tile.GetComponent<ProjectileController>().Rotate();
            tile.GetComponent<ProjectileController>().targetTag = Tags.Player;
            tile.GetComponent<ProjectileController>().power = 5;
        }
        // �ƴ϶�� �÷��̾��� ��ġ�� ���� ���� ��ġ�Ͽ� ������ �Ѵ�.
        else
        {
            GameObject tile = Managers.Pool.Pop(projectile, transform.parent).gameObject;
            tile.GetComponent<Explosion>().power = status.attack;
            tile.transform.position = target.position;
        }
    }

}

