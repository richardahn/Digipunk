using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;



public class CharacterScript : MonoBehaviour
{
    public Animator animator;
    public FiniteStateMachine actions;
    public FiniteStateMachine tangibility;
    public AnimatorController animationMachine;
    public AttackScript attack;


    public const string CHARGE = "ChargeInput";
    public const string RELEASE = "ReleaseInput";
    public const string COMPLETE_CHARGE = "CompleteCharge";
    public const string COMPLETE_MAX_CHARGE = "CompleteMaxCharge";
    public const string COMPLETE_ATTACK = "CompleteAttack";

    public bool isPlayer = true; // Seems kind of pointless when player is defined by whether it has "PlayerInput" attached. 

    // Start is called before the first frame update
    void Start()
    {
        actions = new FiniteStateMachine();
        tangibility = new FiniteStateMachine();

        State cIdle = actions.CreateState("Idle");
        State cCharging = actions.CreateState("Charging"); // Charge, you can let go and it cancels
        State cMaxCharging = actions.CreateState("MaxCharging"); // Max charge, if you let go it fires off, but the longer you hold, the more damage it does, and it eventually has a max duration that you can hold it for and at that point, it lets go automatically
        State cAttacking = actions.CreateState("Attacking");

        cIdle.SetAction(new FloatIdleAction(transform));
        cCharging.SetAction(new ChargeAction(1.0f));
        cMaxCharging.SetAction(new ChargeAction(2.0f)); 
        cAttacking.SetAction(new AttackAction(this, attack, 0.1f));

        cIdle.AddTransition(CHARGE, new Transition(cCharging, new SetTriggerAction(animator, CHARGE)));
        cCharging.AddTransition(RELEASE, new Transition(cIdle, new SetTriggerAction(animator, RELEASE)));
        cCharging.AddTransition(State.COMPLETE, new Transition(cMaxCharging, new SetTriggerAction(animator, COMPLETE_CHARGE)));
        cMaxCharging.AddTransition(RELEASE, new Transition(cAttacking, new SetTriggerAction(animator, COMPLETE_MAX_CHARGE)));
        cMaxCharging.AddTransition(State.COMPLETE, new Transition(cAttacking, new SetTriggerAction(animator, COMPLETE_MAX_CHARGE)));
        cAttacking.AddTransition(State.COMPLETE, new Transition(cIdle));

        //print(animationMachine.parameters[0].name);

    }

    // Update is called once per frame
    void Update()
    {
        actions.Update();
    }
}
