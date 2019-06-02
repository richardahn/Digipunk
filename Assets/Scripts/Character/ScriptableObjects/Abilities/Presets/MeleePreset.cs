using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Preset", menuName = "Ability Preset/Melee Preset")]
public class MeleePreset : AbilityPreset
{
    [SerializeField]
    public float Damage = 30f;
    [SerializeField]
    public float HitstunDuration = 0.1f;
    [SerializeField]
    public float Duration = 0.217f; // Later make a custom editor for this SO and make it so that if you check the bool(Use Animation Duration), then you get an option for an animation clip instead

    public override CharacterAbility Generate(GameObject user, InputKeyStatus key)
    {
        return new MeleeAbility(user, key, this);
    }

}

// Can also put the meleeability in this file