using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AttachableHitbox : BaseHitbox<AttachableHitbox>
{

    // Lifecycle
    private void Awake()
    {
        Initialise(transform.parent.gameObject);
    }

}
