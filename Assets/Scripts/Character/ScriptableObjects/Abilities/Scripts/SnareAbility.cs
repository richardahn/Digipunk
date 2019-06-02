using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnareAbility : CharacterAbility
{
    // Preset
    private SnarePreset preset;

    // Dependents
    protected CharacterInput input; // For miscellaneous input(mouse location, horizontal movement, etc.)

    // State
    protected AbilityState chargeState = new AbilityState();
    protected GameObject launchArcScene;
    protected Vector2 launchVelocity;

    // Animator
    protected readonly int snareParameterHash = Animator.StringToHash("Snare"); // bool

    public SnareAbility(GameObject user, InputKeyStatus key, SnarePreset preset) : base(user, key, preset)
    {
        // Set preset
        this.preset = preset;

        // Setup dependents
        input = user.GetComponent<CharacterInput>();

        // Setup states
        chargeState.OnEnter += SetupLaunchArc;
        chargeState.OnEnterAndUpdate += UpdateLaunchArc;
        chargeState.OnExit += RemoveLaunchArc;
        chargeState.OnExit += LaunchSnare;

        // Setup state machine
        stateMachine.OnMachineEnter += () => anim.SetBool(snareParameterHash, true);
        stateMachine.OnMachineExit += () => anim.SetBool(snareParameterHash, false);
        stateMachine.SetRoot(chargeState);
    }

    protected override void Reset()
    {
        launchArcScene = null;
        launchVelocity = Vector2.zero;
    }

    #region Ability Logic
    protected void SetupLaunchArc()
    {
        // Instantiate prefab
        launchArcScene = MonoBehaviour.Instantiate(preset.LaunchArcPrefab, user.transform);
    }
    protected void UpdateLaunchArc()
    {
        if (key.Released)
        {
            // Move to snare state once key is released
            stateMachine.End();
        }
        else
        {
            // Get direction to mouse
            Vector2 desiredDirection = input.Mouse.Position;
            desiredDirection = Camera.main.ScreenToWorldPoint(desiredDirection);
            Vector2 origin = launchArcScene.transform.position;
            desiredDirection = (desiredDirection - origin).normalized;
            launchVelocity = desiredDirection;
            launchArcScene.GetComponent<LaunchArcRenderer>().SetParameters(launchVelocity, origin);
        }
    }
    protected void RemoveLaunchArc()
    {
        MonoBehaviour.Destroy(launchArcScene); 
    }
    protected void LaunchSnare()
    {
        GameObject snareProjectileScene = MonoBehaviour.Instantiate(preset.SnareProjectilePrefab, user.transform.position, Quaternion.identity);
        snareProjectileScene.GetComponent<LobbedProjectile>().SetParameters(launchVelocity);
        MonoBehaviour.Destroy(snareProjectileScene, 4f);
    }
    #endregion
}
