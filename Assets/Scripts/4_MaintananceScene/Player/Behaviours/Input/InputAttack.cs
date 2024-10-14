using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputAttack : InputBase
{
    public InputAttack()
    {
        Init();
        inputHandler.data[(int)ActionType.Attack].Action = action;
        inputHandler.SubscribeToggle();
    }

    protected override void Init()
    {
        base.Init();
        bindingConfig.Add(("Attack","k"));
//        bindingConfig.Add(("Defense","rightButton"));
//        bindingConfig.Add(("Attack","leftButton"));

        action = new InputAction(nameof(ActionType.Attack), InputActionType.Button);
        action.expectedControlType = nameof(Button);

        bindings = new InputBinding[bindingConfig.Count];
        SetBindings();
    }

    protected override void SetBindings()
    {
        for (int i = 0; i < bindingConfig.Count; i++)
        {
            bindings[i] = new InputBinding();
            bindings[i].name = bindingConfig[i].name;
            bindings[i].action = action.name;
            bindings[i].path = $"<{nameof(InputDevices.Keyboard)}>/{bindingConfig[i].path}";
            action.AddBinding(bindings[i]);
        }
    }
}