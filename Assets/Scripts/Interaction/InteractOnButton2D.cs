using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractOnButton2D : InteractOnTrigger2D
{
    // Parameters
    public UnityEvent OnInteract;

    private void Update()
    {
        if (withinRange && Input.GetKeyDown(KeyCode.F))
        {
            OnInteract?.Invoke();
        }
    }

    // Test
    public void PrintOnInteract()
    {
        Debug.Log("Interacted with Interactable: " + name);

    }
}
