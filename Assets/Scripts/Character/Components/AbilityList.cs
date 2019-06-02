using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// List Items
[System.Serializable]
public abstract class AbilityListItem
{
    public CharacterAbility Ability { get; protected set; }
    [SerializeField]
    protected AbilityPreset preset;

    public abstract void Generate(GameObject user);
    public abstract void UpdateKey();
}
[System.Serializable]
public class AiAbilityListItem : AbilityListItem
{
    public ImitateKeyStatus Key;
    public override void Generate(GameObject user)
    {
        Ability = preset.Generate(user, Key);
    }
    public override void UpdateKey()
    {
        Key.Update();
    }
}
[System.Serializable]
public class PlayerAbilityListItem : AbilityListItem
{
    public PlayerKeyStatus Key;
    public override void Generate(GameObject user)
    {
        Ability = preset.Generate(user, Key);
    }
    public override void UpdateKey()
    {
        Key.Update();
    }
}

// Lists
[System.Serializable]
public class AbilityList<ListItem> where ListItem : AbilityListItem
{
    public List<ListItem> Abilities;

    public void Generate(GameObject user)
    {
        foreach(ListItem item in Abilities)
        {
            item.Generate(user);
        }
    }
    public void Update()
    {
        foreach(ListItem item in Abilities)
        {
            item.UpdateKey();
            item.Ability.Update();
        }
    }
}
[System.Serializable]
public class AiAbilityList : AbilityList<AiAbilityListItem> { }
[System.Serializable]
public class PlayerAbilityList : AbilityList<PlayerAbilityListItem> { }
