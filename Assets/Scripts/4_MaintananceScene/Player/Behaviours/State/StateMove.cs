using System.Collections;
using UnityEngine;

public class StateMove : StateBase
{
    SPUM_Prefabs prefab;
    Player player;
    public StateMove(StateMachine stateMachine) : base(stateMachine)
    {
        player = Managers.Game.player;
        prefab = player.SpumPrefabs;
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
        if (prevState is StateDie or StateInteract)
        {
            Debug.Log($"Cannot transit to {GetType()} from {prevState.GetType()}");
            return false;
        }
        return true;
    }
}