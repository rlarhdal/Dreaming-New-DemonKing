using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : NormalEnemy
{
    [SerializeField]
    private GameObject projectile;
    public bool AttackType = false;

    // 원거리 적에 대한 공격 설정
    private void SpawnProjectile()
    {
        clips.PlaySFX(EnemyClip.Attack);
        // 발사형이라면 플레이어를 향하고 자신의 스탯의 공격력을 설정해준다.
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
        // 아니라면 플레이어의 위치에 폭발 원을 설치하여 공격을 한다.
        else
        {
            GameObject tile = Managers.Pool.Pop(projectile, transform.parent).gameObject;
            tile.GetComponent<Explosion>().power = status.attack;
            tile.transform.position = target.position;
        }
    }

}

