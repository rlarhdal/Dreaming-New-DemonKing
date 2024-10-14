using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenEnemy : Enemy, IDamagable
{
    protected CitizenMovement movement;
    public AudioClips clips;
    
    // 시민의 이벤트 변수
    public Action dieEvent;
    public Action hitEvent;
    public Action moveEvent;
    public Action idleEvent;

    public override void Init(Transform target)
    {
        base.Init(target);
        clips = GetComponent<AudioClips>();
        movement = GetComponent<CitizenMovement>();
    }

    public void ApplyDamage(float dmg)
    {
        clips.PlaySFX(EnemyClip.Hit);
        if (status.health <= dmg)
        {
            status.health -= (int)dmg;
            dieEvent?.Invoke();
        }
        else
        {
            hitEvent?.Invoke();
            status.health -= (int)dmg;
        }
    }
}
