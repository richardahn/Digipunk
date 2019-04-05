using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Brains/Player Brain")]
public class PlayerBrain : CharacterBrain
{
    public readonly string horizontalAxisName = "Horizontal";
    public readonly string verticalAxisName = "Vertical";
    public readonly KeyCode jumpKey = KeyCode.W;

    public override void FixedThink(CharacterThinker character)
    {
        CharacterMover mover = character.GetComponent<CharacterMover>();
        mover.Control(Input.GetAxisRaw(horizontalAxisName), Input.GetKeyDown(jumpKey));
    }

}
