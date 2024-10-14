using Unity.VisualScripting;
using UnityEngine;

public class StateInteract : StateBase
{
    public StateInteract(StateMachine stateMachine) : base(stateMachine)
    {
        layer = StateLayer.Move;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override bool CanTransitState(IState prevState)
    {
        if (prevState is StateDie)
        {
            Debug.Log($"Cannot transit to {GetType()} from {nameof(StateDie)}");
            return false;
        }
        if (prevState is StateDie or StateHit or StateAttack or StateHide or StateSkill)
            return false;
        return true;
    }
}