using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHitbox : AttachableHitbox
{
    // Add script for following the end of the laserhitbox, and add particles at the end of the laser

    // Disable checks for OnTriggerEnter2D()
    private void Start()
    {
        EnterActive = false;
    }
}
