using UnityEngine;

public class StateSkill : StateBase
{
    Player player;
    Coroutine coroutine;
    WaitUntil waitUntil;
    
    public StateSkill(StateMachine stateMachine) : base(stateMachine)
    {
        layer = StateLayer.Attack;
    }

    public override void Enter()
    {
//        Managers.Game.player.Inventory.GetEquippedItem(ItemType.Weapon).UseSkill();
        
    }

    public override void Exit()
    {
    }

    public override bool CanTransitState(IState prevState)
    {
        if (prevState is StateDie)
            return false;
        return true;
    }
}