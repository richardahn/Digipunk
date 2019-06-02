using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Have a list of AbilityStates for an ability

public class AbilityState 
{
    public Action OnEnter;
    public Action OnUpdate;
    public Action OnExit;
    public Action OnEnterAndUpdate;
    public Action OnAll;

    public void Enter()
    {
        OnEnter?.Invoke();
        OnEnterAndUpdate?.Invoke();
        OnAll?.Invoke();
    }
    public void Update()
    {
        OnUpdate?.Invoke();
        OnEnterAndUpdate?.Invoke();
        OnAll?.Invoke();
    }
    public void Exit()
    {
        OnExit?.Invoke();
        OnAll?.Invoke();
    }
}

/*
// Have the waitstate accept a State to go to once duration passes, also retain the ability to pass OnEnter, etc. 
public class WaitState : AbilityState
{
    // StateMachine machine <- i also need a reference to the machine
    protected AbilityState nextState; // I don't like this, instead, have an AbilityState to transition to, and pass that in as a constructor, so it's more easily defined
    // If i want to finish the statemachine, just create an EndState state. 

    protected float duration;
    protected float elapsedTime;

    public WaitState(float duration, AbilityState nextState)
    {
        this.nextState = nextState;
        this.duration = duration;
        elapsedTime = 0f;
        OnEnterAndUpdate += PassTime;
    }
    protected void PassTime()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duration)
        {
            // Transition to nextState
        }
    }
}

public class EndState : AbilityState
{
    public EndState()
    {

    }
}
*/


public class AbilityStateMachine
{
    public AbilityState current;


    public Action OnMachineEnter;
    public Action OnMachineUpdate;
    public Action OnMachineExit;

    public void SetRoot(AbilityState root)
    {
        current = root;
    }

    public void Update()
    {
        current.Update();
        OnMachineUpdate?.Invoke();
    }

    public void Start()
    {
        OnMachineEnter?.Invoke();
        current.Enter();
    }
    public void TransitionTo(AbilityState next)
    {
        current.Exit();
        next.Enter();
        current = next;
    }
    public void End()
    {
        current.Exit();
        OnMachineExit?.Invoke();
    }
}
