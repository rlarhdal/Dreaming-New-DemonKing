using System;
using System.Collections.Generic;
using System.Net;

public class StateMachine
{
//    public IState previousState { get; private set; }
    protected Dictionary<StateLayer,StateBase[]> currentStates;

    public StateMachine()
    {
//        previousState = new StateIdle(this);
        currentStates = new Dictionary<StateLayer, StateBase[]>();
        currentStates[StateLayer.Effects] = new StateBase[(int)StateLayer.Effects];
        currentStates[StateLayer.Move] = new StateBase[(int)StateLayer.Move - (int)StateLayer.Effects];
        currentStates[StateLayer.Attack] = new StateBase[(int)StateLayer.Attack - (int)StateLayer.Move];
        currentStates[StateLayer.Die] = new StateBase[(int)StateLayer.Die - (int)StateLayer.Attack];
        currentStates[StateLayer.Move][0] = new StateIdle(this);
        currentStates[StateLayer.Move][0].Enter();
    }
    public bool ChangeState(StateBase nextState, bool exit = false)
    {
        switch (nextState.layer)
        {
            case StateLayer.Effects:
                if (exit && Array.Exists(currentStates[StateLayer.Effects], state => state == nextState))
                {
                    currentStates[StateLayer.Effects][(int)((StateEffect)nextState).layerNo].Exit();
                    currentStates[StateLayer.Effects][(int)((StateEffect)nextState).layerNo] = null;
                    return false;
                }
                else
                {
                    currentStates[StateLayer.Effects][(int)((StateEffect)nextState).layerNo] = nextState;
                    currentStates[StateLayer.Effects][(int)((StateEffect)nextState).layerNo].Enter();
                    return true;
                }
                break;
            
            case StateLayer.Attack:
                if (currentStates[nextState.layer][0] == null)
                {
                    currentStates[nextState.layer][0] = nextState;
                    currentStates[nextState.layer][0].Enter();
                }
                else if (currentStates[nextState.layer][0] == nextState)
                {
                    if (exit)
                    {
                        currentStates[nextState.layer][0].Exit();
                        currentStates[nextState.layer][0] = null;
                    }
                    return false;
                }
                // deleting below case means each state have to contains enter to exit logic 
//                else
//                {
//                    currentStates[nextState.layer][0].Exit();
//                    currentStates[nextState.layer][0] = nextState;
//                    currentStates[nextState.layer][0].Enter();
//                    return true;
//                }
                break;
            
            case StateLayer.Move:
                if (currentStates[nextState.layer][0] == nextState)
                    return false;
                else
                {
                    currentStates[nextState.layer][0].Exit();
                    currentStates[nextState.layer][0] = nextState;
                    currentStates[nextState.layer][0].Enter();
                    return true;
                }
                break;
            case StateLayer.Die:
                if (exit)
                {
                    currentStates[StateLayer.Die][0].Exit();
                    currentStates[StateLayer.Die][0] = null;
                    return false;
                }
                else
                {
                    currentStates[StateLayer.Die][0] = nextState;
                    currentStates[StateLayer.Die][0].Enter();
                }
                break;
        }
        
//        else if (nextState.CanTransitState(currentStates[nextState.layer]) || force)
//        {
//            currentState?.Exit();
////            previousState = currentState;
//            currentState = nextState;
//            currentState?.Enter();
//            return true;
//        }

        return false;
    }

    // Debugging method
    public StateBase[] GetCurrentStates(StateLayer layer) => currentStates[layer];
}