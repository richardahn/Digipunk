using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A finite state machine that runs on its own and is affected by external input through notifications. 
/// </summary>
public class FiniteStateMachine
{
    private State currentState; 

    /// <summary>
    /// Creates a new state and sets it as the root if the machine was empty.
    /// </summary>
    /// <param name="name">The name of the state</param>
    /// <returns>The newly created state</returns>
    public State CreateState(string name)
    {
        State newState = new State(this, name);
        if (currentState == null)
            SetCurrentState(newState);
        return newState;
    }

    /// <summary>
    /// Notifies the FSM of a desired transition. If the transition is unable to be made, it should be ignored(However, a warning will show up for the purposes of debugging
    /// </summary>
    /// <param name="notification">The notification string is the name of the Transition that was defined when adding it to the State object</param>
    public void Notify(string notification)
    {
        if (currentState == null)
            throw new System.Exception("Attempted to notify an FSM without a current state.");

        // Attempt to transition
        try
        {
            MonoBehaviour.print("Notified state " + currentState.name + " with : " + notification);
            Transition transition = currentState.transitions[notification];
            if (currentState.resetOnExit)
                currentState.Reset();
            transition.OnTransition();
            currentState = transition.destination;
        }
        catch(System.Exception e)
        {
            string msg = e.Message + ", " + "Notified state " + currentState.name + " with notification '" + notification + "'. The only available transitions are: ";
            foreach(KeyValuePair<string, Transition> entry in currentState.transitions)
            {
                msg += entry.Key + ", ";
            }
            Debug.LogWarning(msg);
        }
    }

    public void Update()
    {
        currentState.Update();
    }

    private void SetCurrentState(State state)
    {
        currentState = state;
    }
}


public class State
{
    public const string COMPLETE = "Complete";

    public string name; // Non-unique
    public bool resetOnExit;
    private bool justStarted;
    public Action action; // The action to perform when in this state
    public FiniteStateMachine fsm; // The parent fsm
    public Dictionary<string, Transition> transitions; // A list of outgoing transitions to other states

    public State(FiniteStateMachine fsm, string name)
    {
        transitions = new Dictionary<string, Transition>();
        this.fsm = fsm;
        this.name = name;
        justStarted = true;
        resetOnExit = true;
    }
    public State(FiniteStateMachine fsm, string name, Action action) : this(fsm, name) { SetAction(action); }

    public void SetAction(Action a) { action = a; }

    public void AddTransition(string n, Transition t) { transitions.Add(n, t); }

    public void Update()
    {
        if (justStarted)
        {
            action.OnBegin();
            justStarted = false;
        }

        if (action.Run())
        {
            action.OnEnd();
            fsm.Notify(COMPLETE);
            Reset();
        }

    }

    public void Reset()
    {
        justStarted = true;
        action.Reset();
    }
}


/// <summary>
/// A transition class that defines the destination and the action to perform upon transitioning.
/// </summary>
public class Transition
{
    public readonly State destination;
    private readonly ImmediateAction action;

    public Transition(State destination, ImmediateAction action)
    {
        this.destination = destination;
        this.action = action;
    }

    public Transition(State destination) : this(destination, null) { }

    /// <summary>
    /// Perform an action upon transition, if any.
    /// </summary>
    public void OnTransition()
    {
        if (action != null)
            action.Run();
    }

}
