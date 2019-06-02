using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAbility : ScriptableObject
{
    // Parameters
    private AbilityPreset preset;

    // Dependents
    protected GameObject user;
    protected InputKeyStatus key; // For input related to the specified ability
    protected Animator anim;

    // State
    public bool Active { get; protected set; }
    protected AbilityStateMachine stateMachine = new AbilityStateMachine();

    public CharacterAbility(GameObject user, InputKeyStatus key, AbilityPreset preset)
    {
        // Reset state
        Reset();

        // Get dependents
        this.user = user;
        this.key = key;
        anim = user.GetComponent<Animator>();

        // Reference preset
        this.preset = preset;

        // Setup base machine behaviour
        stateMachine.OnMachineExit += Reset;
    }

    protected virtual void Reset()
    {
        Active = false;
    }
   
    public void Update()
    {
        if (key.Pressed && !Active)
        {
            Debug.Log("Character ability was pressed!");
            Active = true;
            stateMachine.Start();
        }
        else if (Active)
        {
            Debug.Log("Character ability is updating");
            stateMachine.Update();
        }
        else
        {
            Debug.Log("Ability key pressed is: " + key.Pressed + " and active is: " + Active);
        }
    }

    #region Floating Text
    protected virtual void ShowFloatingText(Transform transform, string text)
    {
        if (preset.FloatingTextPrefab != null)
        {
            GameObject floatingTextScene = MonoBehaviour.Instantiate(preset.FloatingTextPrefab, transform.position, Quaternion.identity);
            floatingTextScene.GetComponent<FloatingText>().SetText(text);
        }
    }
    protected virtual void ShowFloatingText(Transform transform, float text)
    {
        ShowFloatingText(transform, Mathf.RoundToInt(text).ToString());
    }
    #endregion
}
