using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedAbility : CharacterAbility
{
    private RangedPreset preset;

    // Animation Parameter Hashes
    protected readonly int rangedParameterHash = Animator.StringToHash("Ranged"); // bool

    // State
    protected AbilityState rangedState = new AbilityState();
    protected float elapsedTime;

    // Public
    public RangedAbility(GameObject user, InputKeyStatus key, RangedPreset preset) : base(user, key, preset)
    {
        // Copy presets
        this.preset = preset;

        // Setup states
        rangedState.OnEnter += LaunchProjectile;

        // Setup state machine
        stateMachine.OnMachineEnter += () => anim.SetBool(rangedParameterHash, true);
        stateMachine.OnMachineExit += () => anim.SetBool(rangedParameterHash, false);
        stateMachine.SetRoot(rangedState);
    }

    protected override void Reset()
    {
        base.Reset();
        elapsedTime = 0f;
    }

    #region Ability Logic
    protected virtual void LaunchProjectile()
    {
        // Launch projectile
        Vector2 direction = user.transform.right * preset.ProjectileSpeed;
        InstantiateProjectile(direction, preset.ProjectileDuration);
    }
    #endregion

    #region Internal helper methods
    protected DetachableHitbox InstantiateProjectile(Vector2 direction, float duration)
    {
        Transform projectileSpawnLocation = user.transform.Find(preset.ProjectileSpawnLocationName);
        DetachableHitbox projectile = MonoBehaviour.Instantiate(preset.ProjectilePrefab, projectileSpawnLocation.position, Quaternion.identity).GetComponent<DetachableHitbox>();

        projectile.Initialise(user);
        projectile.AddEnterListener(HitCharacter);
        projectile.GetComponent<LinearProjectile>().SetParameters(direction, duration);

        return projectile;
    }
    #endregion

    #region Effects
    protected void HitCharacter(DetachableHitbox user, CharacterCore receiver)
    {
        // Apply damage
        ShowFloatingText(receiver.transform, preset.Damage);
        receiver.TakeDamage(preset.Damage);
        receiver.ApplyHitstun(preset.HitstunDuration);
        receiver.GetComponent<CharacterMover>().SetVelocity(Vector2.up * preset.KnockupVelocity);
        Debug.Log("Hit character!");
    }
    #endregion
}
