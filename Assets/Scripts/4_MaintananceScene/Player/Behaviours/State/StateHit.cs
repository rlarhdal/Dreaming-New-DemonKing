using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StateHit : StateEffect
{
    Player player;
    Color[] colors;
    WaitForSeconds hitTime;
    public StateHit(StateMachine stateMachine) : base(stateMachine)
    {
        player = Managers.Game.player;
        player.GetOrAddComponent<PlayerInteraction>().hitEvent += Hit;
        colors = new Color[2];
        colors[0] = Color.white; // default
        colors[1] = Color.red; // hit
        hitTime = new WaitForSeconds(1f);

        layerNo = Effect.Hit;
    }

    public override void Enter()
    {
        animator.SetTrigger("Hit");
    }

    public override void Exit()
    {
    }

    public override bool CanTransitState(IState prevState)
    {
        return true;
    }

    void Hit()
    {
        player.StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        stateMachine.ChangeState(this);
        player.Interaction.ChangeInvincibility(true);
        player.mat.color = colors[1];
        player.Interaction.UpdateHPBar();
        yield return hitTime;
        Debug.Log("invincibility time end");
        player.Interaction.ChangeInvincibility(false);
        player.mat.color = colors[0];
        stateMachine.ChangeState(this, true);
//        stateMachine.ChangeState(player.StateHandler.GetState(ActionType.Idle));
    }
}