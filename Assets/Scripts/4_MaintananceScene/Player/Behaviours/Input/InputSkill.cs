using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSkill : InputBase
{
    public InputSkill()
    {
        Init();
        inputHandler.data[(int)ActionType.Skill].Action = action;
        inputHandler.SubscribeToggle();
    }

    protected override void Init()
    {
        base.Init();
        bindingConfig.Add(("Skill", "semicolon"));
        
        action = new InputAction(nameof(ActionType.Skill), InputActionType.Button);
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