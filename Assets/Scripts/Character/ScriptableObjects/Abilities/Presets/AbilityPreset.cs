using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityPreset : ScriptableObject
{
    [SerializeField]
    public GameObject FloatingTextPrefab;

    public abstract CharacterAbility Generate(GameObject user, InputKeyStatus key);
}
