using UnityEngine;


public class LaserAbility : CharacterAbility
{
    // Preset
    private LaserPreset preset;

    // Dependents
    protected CharacterInput input; // For miscellaneous input(mouse location, horizontal movement, etc.)

    // State
    protected AbilityState chargeState = new AbilityState();
    protected AbilityState fireState = new AbilityState();
    protected LaserHitbox laserHitboxScene;
    protected float currentLaserLength;
    protected float elapsedHitTime;
    protected float elapsedChargeTime;
    protected float elapsedLaserTime;
    protected Vector3 previousLaserDirection;
    protected bool HasPreviousDirection;
    // protected float chargePercentage;

    // Parent Animation Parameter Hashes
    protected readonly int laserParameterHash = Animator.StringToHash("Laser"); // Bool

    // Public
    public LaserAbility(GameObject user, InputKeyStatus key, LaserPreset preset) : base(user, key, preset)
    {
        // Setup dependents
        input = user.GetComponent<CharacterInput>();

        // Reference preset
        this.preset = preset;

        // Setup states
        chargeState.OnEnterAndUpdate += ChargeLaser;
        fireState.OnEnter += SetupHitbox;
        fireState.OnEnterAndUpdate += FireLaser;

        // Setup state machine
        stateMachine.OnMachineEnter += () => anim.SetBool(laserParameterHash, true);
        stateMachine.OnMachineExit += () => anim.SetBool(laserParameterHash, false);
        stateMachine.SetRoot(chargeState);
    }
    
    protected override void Reset()
    {
        base.Reset();
        laserHitboxScene = null;
        currentLaserLength = 0f;
        elapsedHitTime = 0f;
        elapsedChargeTime = 0f;
        elapsedLaserTime = 0f;
        previousLaserDirection = Vector3.zero;
        HasPreviousDirection = false;
    }

    #region Ability Logic
    protected void SetupHitbox()
    {
        LaserHitbox hitbox = user.GetComponentInChildren<LaserHitbox>(true);
        hitbox.Initialise(user);
        hitbox.AddStayListener(HitCharacter);
        laserHitboxScene = hitbox;
    }
    protected void ChargeLaser() 
    {
        elapsedChargeTime += Time.deltaTime;
        if (elapsedChargeTime > preset.MaxChargeTime || key.Released)
        {
            stateMachine.TransitionTo(fireState);
        }
    }
    protected void FireLaser() 
    {
        elapsedLaserTime += Time.deltaTime;
        if (elapsedLaserTime > Mathf.Lerp(preset.MinLaserDuration, preset.MaxLaserDuration, elapsedChargeTime / preset.MaxChargeTime))
        {
            anim.SetBool(laserParameterHash, true);
            stateMachine.End();
            return;
        }

        GameObject laser = user.GetComponentInChildren<LaserHitbox>(true).gameObject;
        Vector2 laserDirection = CalculateLaserDirection();

        // Increase laser length
        currentLaserLength += preset.LaserGrowthSpeed * Time.deltaTime;
        // Cap the laser length with maxLaserDistance
        currentLaserLength = Mathf.Min(currentLaserLength, preset.MaxLaserLength);
        // Cap the laser length with boxcast
        // How to get the laser width? Should I store it in the LaserHitbox script and then retrieve it from GameObject laser?
        RaycastHit2D hit = Physics2D.Raycast(laser.transform.position, laserDirection, currentLaserLength, preset.HitboxLayer.Ground.value);

        if (hit.collider != null)
        {
            float distance = (hit.point - (Vector2)laser.transform.position).magnitude;
            currentLaserLength = Mathf.Min(currentLaserLength, distance);
        }
        // Assign rotation
        laser.transform.right = laserDirection;
        // Assign length
        float laserWidth = Mathf.Lerp(preset.MinLaserWidth, preset.MaxLaserWidth, elapsedChargeTime / preset.MaxChargeTime);
        laser.transform.localScale = new Vector3(currentLaserLength, laserWidth, laser.transform.localScale.y);
    }
    #endregion

    #region Internal helper methods
    protected virtual Vector2 CalculateLaserDirection()
    {
        // [New Direction] Mouse location -> Vector
        Vector3 desiredDirection = input.Mouse.Position;
        desiredDirection = Camera.main.ScreenToWorldPoint(desiredDirection);
        desiredDirection.z = 0f;
        desiredDirection = desiredDirection - user.transform.position;
        desiredDirection.Normalize();

        // Lerp the direction towards it using a max angle delta
        Vector3 previousDirection = previousLaserDirection;
        Vector3 finalDirection = desiredDirection;

        if (HasPreviousDirection)
        {
            finalDirection = Vector3.RotateTowards(previousDirection, desiredDirection, preset.MaxLaserRadianSpeed * Time.deltaTime, 0.0f); // 0.0f means don't change magnitude of the vector
        }
        else
        {
            HasPreviousDirection = true;
        }
        previousLaserDirection = finalDirection;
        return finalDirection;
    }
    #endregion

    #region Effects
    protected void HitCharacter(AttachableHitbox triggerer, CharacterCore receiver)
    {
        elapsedHitTime += Time.deltaTime;
        if (elapsedHitTime > preset.LaserTickDuration)
        {
            float damage = Mathf.Lerp(preset.MinLaserDamage, preset.MaxLaserDamage, elapsedChargeTime / Mathf.Lerp(preset.MinLaserDuration, preset.MaxLaserDuration, elapsedChargeTime / preset.MaxChargeTime));
            ShowFloatingText(receiver.transform, damage);
            receiver.TakeDamage(damage);
            elapsedHitTime = 0f;
        }
    }
    #endregion
}

