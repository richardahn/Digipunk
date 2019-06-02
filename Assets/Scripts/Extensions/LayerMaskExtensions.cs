using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask layer, GameObject other)
    {
        return (layer.value & (1 << other.layer)) != 0;
    }
}
