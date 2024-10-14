using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BehaviourAttack : BehaviourInput
{
    PlayerAnimationEvents animationEvents;
    PlayerStatHandler statHandler;
    LayerMask attackMask;
    public float multiplier;
    void Start()
    {
//        Init();
    }

    public override void Init()
    {
        animationEvents = Managers.Game.player.AnimationEvents;
        animationEvents.attackEvents += Attack;
        
        statHandler = Managers.Game.player.StatHandler;
        attackMask = LayerMask.GetMask("Enemy");
        multiplier = 1f;
        
        // The issue of double trigger at enemy class can be solved by below code
        // the collider configured as trigger (isTrigger) will be ignored in raycast
        Physics2D.queriesHitTriggers = false;
    }

    protected override void Started(InputAction.CallbackContext context)
    {
    }

    protected override void Performed(InputAction.CallbackContext context)
    {
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
    }

    public void Attack()
    {
        var hits = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero,0,attackMask);
        // Cam border and player 
        if (hits.Length != 0)
            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out IDamagable target))
                {
                    float atk = statHandler.GetStat<int>(StatSpecies.ATKPower).value +
                                statHandler.GetStat<int>(StatSpecies.plusATKPower).value;
                    target.ApplyDamage(multiplier*atk);
                }
            }

        multiplier = 1f;
    }
}