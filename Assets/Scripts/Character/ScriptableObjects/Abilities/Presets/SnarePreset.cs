using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Snare Preset", menuName = "Ability Preset/Snare Preset")]
public class SnarePreset : AbilityPreset
{
    // Parameters
    [SerializeField]
    public GameObject SnareProjectilePrefab;
    [SerializeField]
    public GameObject LaunchArcPrefab;

    public override CharacterAbility Generate(GameObject user, InputKeyStatus key)
    {
        return new SnareAbility(user, key, this);
    }
}
