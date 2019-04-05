using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility
{
    public static bool IsPlaying(Animator animator, int layerIndex, string stateName)
    {
        return (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName) &&
                animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1.0f);
    }
}
