using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetachableHitbox : BaseHitbox<DetachableHitbox>
{
    [SerializeField]
    protected GameObject hitParticle;

    // Public    
    protected override void HitCharacterEnter(CharacterCore other)
    {
        Destroy(this.gameObject);
        Instantiate(hitParticle, other.transform);
    }
    protected override void HitDetachableHitboxEnter(DetachableHitbox other)
    {
        GameObject hp = Instantiate(hitParticle);
        hp.transform.position = transform.position;
        //Instantiate(hitParticle, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(other.gameObject);
    }
    protected override void HitGroundEnter()
    {
        GameObject hp = Instantiate(hitParticle);
        hp.transform.position = transform.position;
        //Instantiate(hitParticle, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
