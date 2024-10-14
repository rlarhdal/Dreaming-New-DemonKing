using UnityEngine;

public class StateHide : StateEffect
{
    PlayerInteraction interaction;
    Material mat;
    Color hideColor;
    Color defaultColor;
    public StateHide(StateMachine stateMachine) : base(stateMachine)
    {
        layerNo = Effect.Hide;
        interaction = Managers.Game.player.Interaction;
        mat = Managers.Game.player.mat;

        hideColor = new Color(1f, 1f, 1f, 150f/255f);
        defaultColor = new Color(1f, 1f, 1f, 1f);
    }

    public override void Enter()
    {
        if(interaction == null)
            interaction = Managers.Game.player.Interaction;
        if (mat == null)
            mat = Managers.Game.player.mat;
            
        interaction.invincibility = true;
        mat.color = hideColor;
    }

    public override void Exit()
    {
        interaction.invincibility = false;
        mat.color = defaultColor;
    }

    public override bool CanTransitState(IState prevState)
    {
        if (prevState is StateDie or StateAttack or StateSkill)
        {
            Debug.Log($"Cannot transit {GetType()} from {prevState.GetType()}");
            return false;
        }
        return true;
    }
}