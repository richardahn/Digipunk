using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// TODO: I keep getting an error that happens randomly about index at 0 being null or something, it doesn't happen all the time
public class InteractOnTrigger2D : MonoBehaviour
{
    // Parameters
    public Collider2D interactableCollider;
    public bool interactWithPlayerOnly;
    public string playerTag = "Player";
    public LayerMask interactableLayer;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    // State
    protected bool withinRange;

    private void Awake()
    {
        // If user has defined a collider to use, then use that. If they haven't, then automatically use the one that is attached to the same GameObject.
        if (interactableCollider == null)
        {
            interactableCollider= GetComponent<Collider2D>();
            if (interactableCollider == null)
                Debug.LogError("Could not find a collider for this InteractOnTrigger object with name = " + name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (interactWithPlayerOnly && !collision.CompareTag(playerTag))
            return;
        if (!interactWithPlayerOnly && !interactableLayer.Contains(collision.gameObject))
            return;

        withinRange = true;
        OnEnter?.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactWithPlayerOnly && !collision.CompareTag(playerTag))
            return;
        if (!interactWithPlayerOnly && !interactableLayer.Contains(collision.gameObject))
            return;

        withinRange = false;
        OnExit?.Invoke();
    }

    // Test on event functions
    public void PrintOnEnter()
    {
        Debug.Log("Entered the Interactable: " + name);
    }
    public void PrintOnExit()
    {
        Debug.Log("Exited the Interactable: " + name);
    }
}
