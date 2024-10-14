using UnityEngine;

public class StateIdle : StateBase
{
    SPUM_Prefabs spumPrefabs;
    public StateIdle(StateMachine stateMachine) : base(stateMachine)
    {
        spumPrefabs = Managers.Game.player.SpumPrefabs;
        layer = StateLayer.Move;
    }

    public override void Enter()
    {
//        Debug.Log("State Idle Enter");
//        spumPrefabs.PlayAnimation(nameof(SPUM_AnimClipList.idle));
        animator.SetBool("Idle",true);
    }

    public override void Exit()
    {
//        Debug.Log("State Idle Exit");
        animator.SetBool("Idle",false);
    }

    public override bool CanTransitState(IState prevState)
    {
        if (prevState is StateDie)
            return false;
        return true;
    }
}