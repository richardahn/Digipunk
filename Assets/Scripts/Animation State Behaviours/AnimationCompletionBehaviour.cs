using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCompletionBehaviour : StateMachineBehaviour
{
    public string characterName;
    private CharacterScript character;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character = GameObject.Find(characterName).GetComponent<CharacterScript>();
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        character.actions.Notify(State.COMPLETE);
    }

}
