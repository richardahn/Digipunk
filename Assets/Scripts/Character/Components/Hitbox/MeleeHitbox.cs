using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : AttachableHitbox
{
    // Parameters
    [SerializeField]
    protected GameObject hitParticle;

    // Disable checks for OnTriggerStay2D()
    private void Start()
    {
        StayActive = false;
    }

    protected override void HitCharacterEnter(CharacterCore other)
    {
        Instantiate(hitParticle, other.transform);
        Active = false;
    }

    protected override void HitDetachableHitboxEnter(DetachableHitbox other)
    {
        GameObject hp = Instantiate(hitParticle);
        hp.transform.position = transform.position;
    }
    
}
