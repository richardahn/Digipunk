using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


/*
 * Empty GameObject attack for instant attacks, it has a script attached that looks at the grid and "raycasts"(our simpler version) for a target and summons hit sprite thing on the enemy and changes their shader. 
 * 
 * For attacks, change this script to Character.cs and have an Attack(KeyCode) function, that the InputManager calls. 
 * AI Inputs have to know the state of the character and the state of the enemy. 
 * 
 * Character attacks can be Attack("Name") or Attack(KeyCode)
 * */

public class CharacterScript : MonoBehaviour
{
    public Animator animator;
    public FiniteStateMachine actions;
    public FiniteStateMachine tangibility;
    public AnimationClip clip;

    /*
     * Move this to a script of its own such as Behaviours, then for the clips that you define behaviour for, put them as public animator clip fields.
     * Then you can use the .name member to get its string name, which you can use in IsName().
     * Then from the script, 
     * 
     * Then, in the BehaviourManager : StateMachineBehaviour file, check if IsName("Attack", "Stunned") etc.
     * GetComponent<StunBehaviour>() 
     * 
     * */

    public const string CHARGE = "ChargeInput";
    public const string RELEASE = "ReleaseInput";
    public const string COMPLETE_CHARGE = "CompleteCharge";
    public const string COMPLETE_MAX_CHARGE = "CompleteMaxCharge";
    public const string COMPLETE_ATTACK = "CompleteAttack";
    

    void Awake()
    {
        /*
        print("The name is: " + clip.name);
        FloatIdleAction f = new FloatIdleAction(transform);
        BehaviourHandler bh = new BehaviourHandler();
        bh.OnStateUpdated += (animator, stateInfo, layerIndex) => f.Run();
        behaviours.Add(StateType.Idle, bh);
        // StunEnter -> StunPeak(wait X seconds) -> StunExit
        // CustomBehaviour(w/ state Stunned): set the stunpeak exit transition trigger after waiting X seconds
        //bh.OnStateEntered += (animator, stateInfo, layerIndex) => 
        */
    }
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        /*
        actions = new FiniteStateMachine();
        tangibility = new FiniteStateMachine();


        State cIdle = actions.CreateState("Idle");
        State cCharging = actions.CreateState("Charging"); // Charge, you can let go and it cancels
        State cMaxCharging = actions.CreateState("MaxCharging"); // Max charge, if you let go it fires off, but the longer you hold, the more damage it does, and it eventually has a max duration that you can hold it for and at that point, it lets go automatically
        State cAttacking = actions.CreateState("Attacking");
        // add block state

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

        var controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/Animation/test.controller");
        controller.AddParameter("Transition", AnimatorControllerParameterType.Trigger);
        //print(animationMachine.parameters[0].name);
        */

    }

    // Update is called once per frame
    void Update()
    {
        //actions.Update();

        if (AnimationUtility.IsPlaying(animator, gameObject.layer, clip.name))
            print("THE CHARACTER IS CURRENTLY PLAYING: " + clip.name);
    }
}
