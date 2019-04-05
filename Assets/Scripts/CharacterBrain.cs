using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBrain : ScriptableObject
{
    // For ScriptableObjects(plugin), always pass in the MonoBehaviour(plug), since SO's are an asset, and the MB contains the actual GameObject that is using the SO asset.
    public virtual void Initialize(CharacterThinker character) { }
    public virtual void Think(CharacterThinker character) { }
    public virtual void FixedThink(CharacterThinker character) { }
}
