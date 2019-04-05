using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterThinker : MonoBehaviour
{
    public CharacterBrain brain;
    [HideInInspector] public Memory memory;

    void OnEnable()
    {
        memory = new Memory();
        brain.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        brain.Think(this);
    }

    void FixedUpdate()
    {
        brain.FixedThink(this);
    }
}
