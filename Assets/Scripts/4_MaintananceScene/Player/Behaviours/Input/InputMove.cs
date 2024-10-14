using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMove : InputBase
{
    public InputMove()
    {
        Init();
        inputHandler.data[(int)ActionType.Move].Action = action;
        inputHandler.SubscribeToggle();
    }

    protected override void Init()
    {
        base.Init();

        bindingConfig.Add(("Movement","2DVector"));
        bindingConfig.Add(("Up","w"));
        bindingConfig.Add(("Down","s"));
        bindingConfig.Add(("Left","a"));
        bindingConfig.Add(("Right","d"));
        bindingConfig.Add(("Joystick","leftStick"));

        if (inputHandler.data[(int)ActionType.Move].Action == null)
        {
            action = new InputAction(nameof(ActionType.Move), InputActionType.Value);
            action.expectedControlType = nameof(Vector2);
        }
        else
            action = inputHandler.data[(int)ActionType.Move].Action;

        SetBindings();

    }

    protected override void SetBindings()
    {
        bindings = new InputBinding[bindingConfig.Count];
        for (int i = 0; i < bindingConfig.Count; i++)
        {
            bindings[i] = new InputBinding();
            bindings[i].name = bindingConfig[i].name;
            bindings[i].action = action.name;
            
            if (i == 0) // mother bind
            {
                bindings[i].path = bindingConfig[i].path;
                bindings[i].isComposite = true; // is mother?
                bindings[i].isPartOfComposite = false; // is child?
            }
            else if(i == bindingConfig.Count-1) // joystick
            {
                bindings[i].path = $"<{nameof(InputDevices.Gamepad)}>/{bindingConfig[i].path}";
            }
            else
            {
                bindings[i].path = $"<{nameof(InputDevices.Keyboard)}>/{bindingConfig[i].path}";
                bindings[i].isComposite = false;
                bindings[i].isPartOfComposite = true;
            }

            action.AddBinding(bindings[i]);
        }
    }
}