using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputPotion : InputBase
{
    public InputPotion()
    {
        Init();
        inputHandler.data[(int)ActionType.Potion].Action = action;
        inputHandler.SubscribeToggle();
    }

    protected override void Init()
    {
        base.Init();
        bindingConfig.Add(("Potion","quote"));

        action = new InputAction(nameof(ActionType.Potion), InputActionType.Button);
        action.expectedControlType = nameof(Button);

        bindings = new InputBinding[bindingConfig.Count];
        SetBindings();
    }
    protected override void SetBindings()
    {
        for (int i = 0; i < bindingConfig.Count; i++)
        {
            bindings[i] = new InputBinding();
            // To do
            bindings[i].name = bindingConfig[i].name;
            bindings[i].action = action.name;
            bindings[i].path = $"<{nameof(InputDevices.Keyboard)}>/{bindingConfig[i].path}";
            action.AddBinding(bindings[i]);
        }
    }
}