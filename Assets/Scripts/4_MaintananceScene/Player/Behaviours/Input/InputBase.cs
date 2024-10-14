using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public abstract class InputBase
{
    protected InputAction action;
    protected InputBinding[] bindings;
    protected List<(string name, string path)> bindingConfig;
    protected PlayerInputHandler inputHandler;

    protected virtual void Init()
    {
        inputHandler = Managers.Game.player.InputHandler;
        bindingConfig = new List<(string name, string path)>();
    }

    protected abstract void SetBindings();
}