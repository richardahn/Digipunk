using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AimedRangedAbility : RangedAbility
{
    private AimedRangedPreset preset;

    public AimedRangedAbility(GameObject user, InputKeyStatus key, AimedRangedPreset preset) : base(user, key, preset)
    {
        // Reference preset
        this.preset = preset;
    }

    // State behaviour
    protected override void LaunchProjectile()
    {
        Collider2D enemy = Physics2DUtility.FindClosestTarget(user, preset.SearchRange, preset.HitboxLayer.Character);
        CharacterMover mover = user.GetComponent<CharacterMover>(); // TODO: Can cache this if you need to

        // Float
        //user.GetComponent<CharacterCore>().ApplyFloat(floatDuration, floatDuration);

        // Initialise projectile
        Vector2 direction = user.transform.right * preset.ProjectileSpeed;
        if (enemy != null)
        {
            direction = (enemy.transform.position - user.transform.position).normalized * preset.ProjectileSpeed;

            // Change facing direction of user
            if (enemy.transform.position.x > user.transform.position.x) // If enemy is to the right of user, face right
            {
                mover.FaceRight(true);
            }
            else
            {
                mover.FaceRight(false);
            }
        }

        InstantiateProjectile(direction, preset.ProjectileDuration).transform.right = direction;
    }
}
