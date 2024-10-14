using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateAttack : StateBase
{
    Player player;
    Coroutine coroutine;
    WaitUntil waitUntil;
    static readonly int coolDownState = Animator.StringToHash("AttackCoolDown");
    static readonly int triggerHash = Animator.StringToHash("Attack");
    public StateAttack(StateMachine stateMachine) : base(stateMachine)
    {
        player = Managers.Game.player;
        layer = StateLayer.Attack;
        waitUntil = new WaitUntil(() => animator.GetBool(coolDownState)==false);
    }

    public override void Enter()
    {
        if(coroutine == null)
            player.StartCoroutine(CoolDownCoroutine());
    }
    public override void Exit()
    {
    }

    public override bool CanTransitState(IState prevState)
    {
        if (prevState is StateDie)
        {
            Debug.Log($"Cannot transit {GetType()} from {prevState.GetType()}");
            return false;
        }
        return true;
    }

    private IEnumerator CoolDownCoroutine()
    {
        yield return waitUntil;
        coroutine = null;
        stateMachine.ChangeState(this, true);
    }
}