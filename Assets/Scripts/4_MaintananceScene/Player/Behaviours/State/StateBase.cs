using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : IState
{
    protected StateMachine stateMachine;
    public StateLayer layer;
    protected Animator animator;

    public StateBase(StateMachine stateMachine)
    {
        layer = StateLayer.None;
        this.stateMachine = stateMachine;
        animator = Managers.Game.player.GetComponentInChildren<Animator>();
    }

    public abstract void Enter();

    public abstract void Exit();
    public abstract bool CanTransitState(IState prevState);
}

public abstract class StateEffect : StateBase
{
    public Effect layerNo = Effect.None;
    protected StateEffect(StateMachine stateMachine) : base(stateMachine)
    {
        layer = StateLayer.Effects;
    }
}