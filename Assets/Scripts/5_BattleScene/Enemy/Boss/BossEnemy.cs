using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy, IDamagable
{
    // 보스 이벤트 변수
    public Action attack1Event;
    public Action attack2Event;
    public Action attack3Event;
    public Action attack4Event;
    public Action special1Event;
    public Action special2Event;
    public Action dieEvent;
    public Action stunEvent;

    // 보스 컴포넌트
    public BossMovement movement;
    BossSkill skill;

    // 타겟의 거리 캐싱
    private Vector3 direction;

    void Start()
    {
        Init(Managers.Game.player.transform);
    }

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<BossMovement>();
        skill = GetComponent<BossSkill>();
    }

    // 타겟과 자신의 거리를 반환
    public Vector2 Direction()
    {
        if (target == null)
            target = Managers.Game.player.transform;
        direction = target.transform.position - transform.position;
        return direction;
    }

    // 피격 시 데미지 입게 한다.
    public void ApplyDamage(float dmg)
    {
        // 입는 데미지가 체력보다 크거나 같다면 죽게함
        if (status.health <= dmg)
        {
            status.health = 0;

            // 죽음
            dieEvent?.Invoke();
            skill.StopAllCoroutines();
            movement.rigid.velocity = Vector3.zero;

            // 스테이지 클리어
            MapGenerator.instance.StageClear();
        }
        // 아니라면 데미지만 입게 구현
        else
        {
            if (skill.specialHit)
            {
                skill.specialCount++;
                status.health -= 1;
            }
            else
            {
                status.health -= (int)dmg;
            }
            // 보스 UI에서 보스가 데미지를 입을 때마다 UI를 업데이트 하게 함
            MapGenerator.instance.Hit(maxHealth, status.health);
        }
    }
}