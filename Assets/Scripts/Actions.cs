using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public abstract bool Run();
    public virtual void Reset() { }
    public virtual void OnBegin() { }
    public virtual void OnEnd() { }
}

public class ImmediateAction : Action
{
    public override bool Run() { return true; }
}

public class SetTriggerAction : ImmediateAction
{
    public Animator animator;
    public string trigger;
    public SetTriggerAction(Animator a, string t)
    {
        animator = a;
        trigger = t;
    }
    public override bool Run()
    {
        MonoBehaviour.print("Printing trigger: " + trigger);
        animator.SetTrigger(trigger);
        return base.Run();
    }
}

public class CustomAction : ImmediateAction
{
    public delegate void Task();
    public Task task;

    public CustomAction(Task t) { task = t; }
    public override bool Run()
    {
        task();
        return base.Run();
    }
}

public class NoAction : Action
{
    public override bool Run() { return false; }
}

public class FloatIdleAction : NoAction
{
    private Transform transform;
    private float elapsedTime = 0.0f;
    private readonly float clamp = 3f;
    private readonly float speed = 2.5f;
    private readonly float loopThreshold;
    private readonly Vector3 originalTransform;

    public FloatIdleAction(Transform transform)
    {
        this.transform = transform;
        originalTransform = transform.position;
        loopThreshold = 2f * Mathf.PI / speed;
    }
    private void Iterate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= loopThreshold)
            elapsedTime = 0.0f;
    }
    private void Move()
    {
        transform.position = originalTransform + new Vector3(0f, (1f/clamp) * Mathf.Sin(speed * elapsedTime), 0f);
    }
    public override bool Run()
    {
        Iterate();
        Move();
        return base.Run();
    }
    // I should define a reset function but not have it get called by state. Reset should set the transform to original transform, set elapsed time to 0. 
}

public class WaitAction : Action
{
    private float elapsedTime;
    private readonly float runTime;
    public WaitAction(float rt)
    {
        runTime = rt;
        Reset();
    }
    public override bool Run()
    {
        elapsedTime += Time.deltaTime;
        return elapsedTime >= runTime;
    }
    public override void Reset() { elapsedTime = 0f; }
}

public class ChargeAction : WaitAction
{
    // Once you hold the key for 5 seconds, move to attack state where u send out the attack

    public ChargeAction(float maxCharge) : base(maxCharge) { } // Instead of taking the charge duration as a parameter, take the Stats class as a parameter since the charge time will be dependent on your stats

}

public class AttackAction : NoAction
{
    private readonly CharacterScript instance;
    private readonly AttackScript attack;
    private readonly float attackDelay;
    public AttackAction(CharacterScript instance, AttackScript attack, float attackDelay) 
    {
        this.instance = instance;
        this.attackDelay = attackDelay;
    }
    public override void OnBegin()
    {
        instance.StartCoroutine(instantiateAttack());
    }
    private IEnumerator instantiateAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        Object.Instantiate(instance.attack);
    }
}

