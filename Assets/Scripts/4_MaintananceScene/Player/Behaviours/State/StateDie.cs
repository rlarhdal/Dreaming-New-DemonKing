using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StateDie : StateBase
{
    PlayerAnimationEvents animationEvents;
    Rigidbody2D rb2D;
    public StateDie(StateMachine stateMachine) : base(stateMachine)
    {
        layer = StateLayer.Die;
        animationEvents = Util.FindChild(Managers.Game.player.gameObject, "UnitRoot",true)
            .GetOrAddComponent<PlayerAnimationEvents>();
//            Managers.Game.player.GetOrAddComponent<PlayerAnimationEvents>();
        animationEvents.dieEvents += Die;
        rb2D = Managers.Game.player.GetOrAddComponent<Rigidbody2D>();
    }

    public override void Enter()
    {
        rb2D.velocity = Vector2.zero;
        animator.SetBool("Death",true);
    }

    public override void Exit()
    {
        animator.SetBool("Death",false);
    }

    public override bool CanTransitState(IState prevState)
    {
        return true;
    }

    void Die()
    {
        Managers.Game.player.StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        if (Managers.Game.player.StateHandler.stateMachine.GetCurrentStates(StateLayer.Die)[0] is not StateDie)
        {
            stateMachine.ChangeState(this);
            yield return new WaitForSeconds(1.5f);
            // Respawn popUp UI should be called
            Managers.UI.ShowPopupUI<UI_Respawn>();
            Time.timeScale = 0f;
        }
    }
}