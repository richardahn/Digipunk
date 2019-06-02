using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseHitbox<T> : MonoBehaviour where T : BaseHitbox<T>
{
    // State
    public bool Active { get; protected set; } = true;
    public bool EnterActive { get; protected set; } = true;
    public bool StayActive { get; protected set; } = true;

    // Layer parameter
    [SerializeField]
    protected LayerMask attachedHitboxLayer;
    [SerializeField]
    protected LayerMask detachedHitboxLayer;
    [SerializeField]
    protected LayerMask characterLayer;
    [SerializeField]
    protected LayerMask groundLayer;

    // Dependents
    public GameObject User { get; protected set; } // Reference is necessary to prevent self-collision, etc.
    public GameObject Character { get { return User.gameObject; } } // To simplify

    #region Events
    protected event Action<T, CharacterCore> OnHitCharacterEnter;
    protected event Action<T, DetachableHitbox> OnHitDetachableHitboxEnter;
    protected event Action<T, AttachableHitbox> OnHitAttachableHitboxEnter;
    protected event Action<T> OnHitGroundEnter;

    protected event Action<T, CharacterCore> OnHitCharacterStay;
    protected event Action<T, DetachableHitbox> OnHitDetachableHitboxStay;
    protected event Action<T, AttachableHitbox> OnHitAttachableHitboxStay;
    protected event Action<T> OnHitGroundStay;

    public void AddEnterListener(Action<T, CharacterCore> callback) { OnHitCharacterEnter += callback; }
    public void AddEnterListener(Action<T, DetachableHitbox> callback) { OnHitDetachableHitboxEnter += callback; }
    public void AddEnterListener(Action<T, AttachableHitbox> callback) { OnHitAttachableHitboxEnter += callback; }
    public void AddEnterListener(Action<T> callback) { OnHitGroundEnter += callback; }

    public void AddStayListener(Action<T, CharacterCore> callback) { OnHitCharacterStay += callback; }
    public void AddStayListener(Action<T, DetachableHitbox> callback) { OnHitDetachableHitboxStay += callback; }
    public void AddStayListener(Action<T, AttachableHitbox> callback) { OnHitAttachableHitboxStay += callback; }
    public void AddStayListener(Action<T> callback) { OnHitGroundStay += callback; }

    public void RemoveAllListeners()
    {
        OnHitCharacterEnter = null;
        OnHitDetachableHitboxEnter = null;
        OnHitAttachableHitboxEnter = null;
        OnHitGroundEnter = null;

        OnHitCharacterStay = null;
        OnHitDetachableHitboxStay = null;
        OnHitAttachableHitboxStay = null;
        OnHitGroundStay = null;
    }
    #endregion

    // Public
    public void Initialise(GameObject user) // TODO: why do I need user? 
    {
        User = user;
        Reset();
    }
    public void Reset()
    {
        Active = true;
    }

    // Internal - collision
    private void OnTriggerEnter2D(Collider2D collision) // This is only for entering
    {
        if (!Active || !EnterActive)
            return;

        if (ReferenceEquals(User, collision.gameObject)) // If I collided with the user, ignore
            return;
        

        bool successfulHit = false;
        if (attachedHitboxLayer.Contains(collision.gameObject))
        {
            AttachableHitbox other = collision.GetComponent<AttachableHitbox>();
            OnHitAttachableHitboxEnter?.Invoke((T)this, other);
            HitAttachableHitboxEnter(other);
            successfulHit = true;
        }
        else if (detachedHitboxLayer.Contains(collision.gameObject))
        {
            DetachableHitbox other = collision.GetComponent<DetachableHitbox>();
            if (GetInstanceID() > other.GetInstanceID())
            {
                OnHitDetachableHitboxEnter?.Invoke((T)this, other); // Apply Destroy() within the Effect
                HitDetachableHitboxEnter(other);
                successfulHit = true;
            }
        }
        else if (characterLayer.Contains(collision.gameObject))
        {
            CharacterCore other = collision.GetComponent<CharacterCore>();
            OnHitCharacterEnter?.Invoke((T)this, other);
            HitCharacterEnter(other);
            successfulHit = true;
        }
        else if (groundLayer.Contains(collision.gameObject))
        {
            OnHitGroundEnter?.Invoke((T)this);
            HitGroundEnter();
            successfulHit = true;
        }

        // General behaviour
        if (successfulHit)
            HitAnythingEnter();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Active || !StayActive)
            return;

        if (ReferenceEquals(User, collision.gameObject)) // If I collided with the user, ignore
            return;


        bool successfulHit = false;
        if (attachedHitboxLayer.Contains(collision.gameObject))
        {
            AttachableHitbox other = collision.GetComponent<AttachableHitbox>();
            OnHitAttachableHitboxStay?.Invoke((T)this, other);
            HitAttachableHitboxStay(other);
            successfulHit = true;
        }
        else if (detachedHitboxLayer.Contains(collision.gameObject))
        {
            DetachableHitbox other = collision.GetComponent<DetachableHitbox>();
            if (GetInstanceID() > other.GetInstanceID())
            {
                OnHitDetachableHitboxStay?.Invoke((T)this, other); // Apply Destroy() within the Effect
                HitDetachableHitboxStay(other);
                successfulHit = true;
            }
        }
        else if (characterLayer.Contains(collision.gameObject))
        {
            CharacterCore other = collision.GetComponent<CharacterCore>();
            OnHitCharacterStay?.Invoke((T)this, other);
            HitCharacterStay(other);
            successfulHit = true;
        }
        else if (groundLayer.Contains(collision.gameObject))
        {
            OnHitGroundStay?.Invoke((T)this);
            HitGroundStay();
            successfulHit = true;
        }

        // General behaviour
        if (successfulHit)
            HitAnythingStay();
    }

    protected virtual void HitAnythingEnter() { }
    protected virtual void HitCharacterEnter(CharacterCore other) { }
    protected virtual void HitDetachableHitboxEnter(DetachableHitbox other) { }
    protected virtual void HitAttachableHitboxEnter(AttachableHitbox other) { }
    protected virtual void HitGroundEnter() { }

    protected virtual void HitAnythingStay() { }
    protected virtual void HitCharacterStay(CharacterCore other) { }
    protected virtual void HitDetachableHitboxStay(DetachableHitbox other) { }
    protected virtual void HitAttachableHitboxStay(AttachableHitbox other) { }
    protected virtual void HitGroundStay() { }

}