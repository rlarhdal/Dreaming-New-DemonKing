using UnityEngine;
using UnityEngine.InputSystem;

public class BehaviourInteract : BehaviourInput
{
    PlayerInputHandler handler;
    NPC interactNPC;
    StateBase state;
    LayerMask npcMask;
    public override void Init()
    {
        input = new InputInteract();
        npcMask = LayerMask.GetMask("NPC");
        stateMachine = Managers.Game.player.StateHandler.stateMachine;
        state = Managers.Game.player.StateHandler.GetState(ActionType.Interact);

        handler = Managers.Game.player.InputHandler;
        handler.data[(int)ActionType.Interact].Subscribers[InputStatus.Started] += Started;
        handler.SubscribeToggle();
    }

    protected override void Started(InputAction.CallbackContext context)
    {
        if (interactNPC != null)
        {
            stateMachine.ChangeState(state);
            interactNPC.ShowUI();
        }
    }

    protected override void Performed(InputAction.CallbackContext context)
    {
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
    }

    public void SetInteractNPC(NPC npc)
    {
        interactNPC = npc;
    }
}