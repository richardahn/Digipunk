using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Laser Preset", menuName = "Ability Preset/Laser Preset")]
public class LaserPreset : AbilityPreset
{
    [Header("Charge State Parameters")]
    [SerializeField]
    public float MaxChargeTime = 5f;
    [Header("Laser State Parameters")]
    [SerializeField]
    public float MinLaserDamage = 2f;
    [SerializeField]
    public float MaxLaserDamage = 10f;
    [SerializeField]
    public float MinLaserWidth = 0.2f;
    [SerializeField]
    public float MaxLaserWidth = 15f;
    [SerializeField]
    public float MinLaserDuration = 1f;
    [SerializeField]
    public float MaxLaserDuration = 2f;
    [SerializeField]
    public float LaserGrowthSpeed = 300f;
    [SerializeField]
    public float LaserTickDuration = 0.5f;
    [SerializeField]
    public float MaxLaserLength = 30f;
    [SerializeField]
    public float MaxLaserRadianSpeed = 2f;
    // Hitbox layers
    [SerializeField]
    public HitboxLayer HitboxLayer;

    public override CharacterAbility Generate(GameObject user, InputKeyStatus key)
    {
        return new LaserAbility(user, key, this);
    }
}
