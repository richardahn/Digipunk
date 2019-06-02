using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that encapsulates attributes TODO: Rename this to CharacterCore
public class CharacterCore : MonoBehaviour
{
    // Parameters
    [SerializeField]
    protected float maxHealth = 100f;
    [SerializeField]
    protected float currentHealth;

    // Dependents
    protected Animator anim;
    protected CharacterMover mover;

    // State
    protected Coroutine hitstunRoutine;
    protected Coroutine floatRoutine;

    // Lifecycle
    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        mover = GetComponent<CharacterMover>();
    }

    // Public
    public float GetPercentHealth()
    {
        return currentHealth / maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            // Die
            Debug.Log(name + " died");
        }
    }

    public void ApplyFloat(float floatDuration, float slowGravDuration)
    {
        if (floatRoutine != null)
        {
            StopCoroutine(floatRoutine);
        }
        floatRoutine = StartCoroutine(FloatRoutine(floatDuration, slowGravDuration));
    }

    public void ApplyHitstun(float duration)
    {
        if (hitstunRoutine != null)
        {
            StopCoroutine(hitstunRoutine);
        }
        hitstunRoutine = StartCoroutine(HitstunRoutine(duration)); // TODO: Stop Coroutine if it exists
    }
    // Disable Animator(behaviours that implement Update) and disable Components(that implement FixedUpdate)
    public IEnumerator HitstunRoutine(float duration)
    {
        anim.enabled = false;
        mover.enabled = false;
        yield return new WaitForSeconds(duration);
        anim.enabled = true;
        mover.enabled = true;
    }

    public IEnumerator FloatRoutine(float floatDuration, float slowGravDuration)
    {
        mover.SetVelocity(Vector2.zero);
        mover.enabled = false;
        yield return new WaitForSeconds(floatDuration);
        mover.enabled = true;
        float oldGravity = mover.gravity;
        mover.gravity = oldGravity * 0.5f;
        yield return new WaitForSeconds(slowGravDuration);
        mover.gravity = oldGravity;


    }

}
