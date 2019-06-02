using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : CharacterInput
{
    public float elapsedTime = 0f;
    public float shootInterval = 1f;
    public bool disabled = true;

    public AiAbilityList aiAbilityList;
    
    protected override void PreUpdate()
    {
        if (disabled)
            return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime > shootInterval)
        {
            // Shoot
            elapsedTime = 0f;
        }
    }
}
