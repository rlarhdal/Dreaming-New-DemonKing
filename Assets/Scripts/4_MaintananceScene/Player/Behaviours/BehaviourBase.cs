using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class BehaviourBase : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected StateBase state;
//    protected UnityAction[] action;

    public abstract void Init();
}

public abstract class BehaviourInput : BehaviourBase
{
    protected InputBase input;

    protected abstract void Started(InputAction.CallbackContext context);
    protected abstract void Performed(InputAction.CallbackContext context);
    protected abstract void Canceled(InputAction.CallbackContext context);
}