using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputInteract : InputBase
{
    public InputInteract()
    {
        Init();
        inputHandler.data[(int)ActionType.Interact].Action = action;
        inputHandler.SubscribeToggle();
    }

    protected override void Init()
    {
        base.Init();
        bindingConfig.Add(("Interact","l"));
        
        action = new InputAction(nameof(ActionType.Interact),InputActionType.Button);
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