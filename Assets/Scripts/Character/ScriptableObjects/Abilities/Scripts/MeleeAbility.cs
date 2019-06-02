using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: dont change any functionality right now, but after refactoring, change the hitbox activation and deactivation in code, so like: float windup = 0.3f, float attack = 0.5f or something
// maybe have a scroll wheel thingy for the editor, to choose from 0 to 1 or something, and translate that to time in seconds, not just 0 to 1.

public class MeleeAbility : CharacterAbility
{
    // Data
    private MeleePreset preset;

    // State
    protected AbilityState meleeState = new AbilityState();
    protected MeleeHitbox meleeHitboxScene;
    protected float elapsedTime;

    // Animation Parameter Hashes
    protected int meleeParameterHash = Animator.StringToHash("Melee"); // Bool

    public MeleeAbility(GameObject user, InputKeyStatus key, MeleePreset preset) : base(user, key, preset)
    {
        // Reference preset
        this.preset = preset;

        // Setup states
        meleeState.OnEnter += SetupHitbox;
        meleeState.OnEnterAndUpdate += PassTime;
        meleeState.OnExit += RemoveHitbox;

        // Setup state machine
        stateMachine.OnMachineEnter += () => anim.SetBool(meleeParameterHash, true);
        stateMachine.OnMachineExit += () => anim.SetBool(meleeParameterHash, false);
        stateMachine.SetRoot(meleeState);
    }
    
    protected override void Reset()
    {
        base.Reset();
        elapsedTime = 0f;
    }

    #region Ability Logic
    protected void PassTime()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > preset.Duration)
            stateMachine.End();
    }
    protected void SetupHitbox()
    {
        // Instantiate melee prefab
        MeleeHitbox hitbox = user.GetComponentInChildren<MeleeHitbox>(true);
        hitbox.Reset(); // TODO: fix this this is bad
        hitbox.AddEnterListener(HitCharacter);
        hitbox.AddEnterListener(HitAttachableHitbox);
        hitbox.AddEnterListener(HitDetachableHitbox);
        meleeHitboxScene = hitbox;
    }
    protected void RemoveHitbox()
    {
        meleeHitboxScene?.RemoveAllListeners();
    }
    #endregion

    #region Effects
    protected void HitCharacter(AttachableHitbox user, CharacterCore receiver)
    {
        // Apply damage
        ShowFloatingText(receiver.transform, preset.Damage);
        receiver.TakeDamage(preset.Damage);
        user.Character.GetComponent<CharacterCore>().ApplyHitstun(preset.HitstunDuration);
        receiver.ApplyHitstun(preset.HitstunDuration);

    }
    protected void HitAttachableHitbox(AttachableHitbox user, AttachableHitbox receiver)
    {
        // Parry
        user.Character.GetComponent<CharacterCore>().ApplyHitstun(preset.HitstunDuration);
        receiver.Character.GetComponent<CharacterCore>().ApplyHitstun(preset.HitstunDuration);
    }
    protected void HitDetachableHitbox(AttachableHitbox user, DetachableHitbox receiver)
    {
        // Destroy projectile(or deflect projectile + hitstun the projectile too)
        MonoBehaviour.Destroy(receiver.gameObject);

        // Apply hitstun
        user.Character.GetComponent<CharacterCore>().ApplyHitstun(preset.HitstunDuration);
    }
    #endregion


}
