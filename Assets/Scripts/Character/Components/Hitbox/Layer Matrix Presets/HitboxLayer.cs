using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hitbox Layer", menuName = "Hitbox Layer")]
public class HitboxLayer : ScriptableObject
{
    public LayerMask AttachedHitbox;
    public LayerMask DetachedHitbox;
    public LayerMask Character;
    public LayerMask Ground;
}
