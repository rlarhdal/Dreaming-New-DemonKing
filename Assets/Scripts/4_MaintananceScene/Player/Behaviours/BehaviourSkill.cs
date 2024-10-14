using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BehaviourSkill : BehaviourInput
{
    PlayerAnimationEvents animationEvents;
    PlayerStatHandler statHandler;
    LayerMask skillMask;
    public Item item;
    float skillRange;
    void Start()
    {
//        Init();
    }

    public override void Init()
    {
        skillRange = 5f;
        item = Managers.Game.player.Inventory.GetEquippedItem(ItemType.Weapon);
        animationEvents = Managers.Game.player.AnimationEvents;
        statHandler = Managers.Game.player.StatHandler;
        skillMask = LayerMask.GetMask("Enemy");
        
        input = new InputSkill();
        stateMachine = Managers.Game.player.StateHandler.stateMachine;
        state = Managers.Game.player.StateHandler.GetState(ActionType.Skill);

        PlayerInputHandler handler = Managers.Game.player.InputHandler;
        handler.data[(int)ActionType.Skill].Subscribers[InputStatus.Started] += Started;
        handler.SubscribeToggle();
    }

    public void UseWeaponSkill()
    {
        if(item != null)
            item.UseSkill();
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    protected override void Started(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(state);
    }

    protected override void Performed(InputAction.CallbackContext context)
    {
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
//        stateMachine.ChangeState(stateMachine.previousState);
    }

    public void AddSkill(Item item)
    {
        if (this.item == null || this.item != item)
            this.item = item;
    }
}