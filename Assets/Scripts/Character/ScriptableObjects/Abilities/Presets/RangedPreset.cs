using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Preset", menuName = "Ability Preset/Ranged Preset")]
public class RangedPreset : AbilityPreset
{
    [Header("Ability Settings")]
    [SerializeField]
    public float Damage = 20f;
    [SerializeField]
    public float Duration = 0.3f;
    [SerializeField]
    public float HitstunDuration = 0.1f;
    [SerializeField]
    public float KnockupVelocity = 20f;
    [Header("Projectile Settings")]
    [SerializeField]
    public GameObject ProjectilePrefab;
    [SerializeField]
    public float ProjectileSpeed = 55f;
    [SerializeField]
    public float ProjectileDuration = 4f;
    [SerializeField]
    public string ProjectileSpawnLocationName = "ProjectileSpawnLocation";

    public override CharacterAbility Generate(GameObject user, InputKeyStatus key)
    {
        return new RangedAbility(user, key, this);
    }
}
