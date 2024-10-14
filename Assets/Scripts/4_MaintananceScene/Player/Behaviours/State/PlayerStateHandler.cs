using UnityEngine;

public class PlayerStateHandler 
{
    public readonly StateMachine stateMachine;

    StateBase[] states;

    public PlayerStateHandler()
    {
        Managers.Game.player.StateHandler = this;
        stateMachine = new StateMachine();

        states = new StateBase[(int)ActionType.Count];
        states[(int)ActionType.Idle] = new StateIdle(stateMachine);
        states[(int)ActionType.Hit] = new StateHit(stateMachine);
        states[(int)ActionType.Die] = new StateDie(stateMachine);
        states[(int)ActionType.Move] = new StateMove(stateMachine);
        states[(int)ActionType.Attack] = new StateAttack(stateMachine);
        states[(int)ActionType.Hide] = new StateHide(stateMachine);
        states[(int)ActionType.Skill] = new StateSkill(stateMachine);
        states[(int)ActionType.Interact] = new StateInteract(stateMachine);
    }

    public StateBase GetState(ActionType type)
    {
        if (states[(int)type] is null)
        {
            Debug.Log($"There's no instance of {nameof(type)}in states");
            return null;
        }

        return states[(int)type];
    }
}