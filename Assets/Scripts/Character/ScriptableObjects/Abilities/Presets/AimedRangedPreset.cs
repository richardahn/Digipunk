using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aimed Ranged Preset", menuName = "Ability Preset/Aimed Ranged Preset")]
public class AimedRangedPreset : RangedPreset
{
    [Header("Aiming Settings")]
    [SerializeField]
    public float SearchRange = 30f;
    [SerializeField]
    public HitboxLayer HitboxLayer;

    public override CharacterAbility Generate(GameObject user, InputKeyStatus key)
    {
        return new AimedRangedAbility(user, key, this);
    }
}
