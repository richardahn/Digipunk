using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    // State
    public InputAxisStatus Horizontal { get; protected set; } = InputAxisStatus.None;
    public InputMouseStatus Mouse { get; protected set; } = InputMouseStatus.None;
    public InputKeyStatus Interact { get; protected set; } = InputKeyStatus.None;
    public InputKeyStatus Jump { get; protected set; } = InputKeyStatus.None;

    protected virtual void PreUpdate() { }
    protected virtual void PostUpdate() { }

    private void Update()
    {
        PreUpdate();

        Horizontal.Update();
        Mouse.Update();
        Jump.Update();
        Interact.Update();

        PostUpdate();
    }
}

public class InputMouseStatus
{
    public static InputMouseStatus None { get; private set; } = new InputMouseStatus();
    public Vector2 Position { get; protected set; }

    public virtual void Update() { }
}
public class ImitateMouseStatus : InputMouseStatus
{
    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}
public class PlayerMouseStatus : InputMouseStatus
{
    public override void Update()
    {
        Position = Input.mousePosition;
    }
}
public class InputAxisStatus
{
    public static InputAxisStatus None { get; private set; } = new InputAxisStatus();
    public float Value { get; protected set; }

    public virtual void Update() { }
}
public class ImitateAxisStatus : InputAxisStatus
{
    public void SetValue(float value)
    {
        Value = value;
    }
}
public class PlayerAxisStatus : InputAxisStatus
{
    public string Direction { get; protected set; }
    public PlayerAxisStatus(string direction)
    {
        Direction = direction;
    }
    public override void Update()
    {
        Value = Input.GetAxisRaw(Direction);
    }
}
public class InputKeyStatus
{
    public static InputKeyStatus None { get; private set; } = new InputKeyStatus();
    public bool Pressed { get; protected set; }
    public bool Held { get; protected set; }
    public bool Released { get; protected set; }

    public virtual void Update() { }
}
public class ImitateKeyStatus : InputKeyStatus
{
    private bool queuePress;
    private bool queueRelease;

    public override void Update()
    {
        if (queuePress)
        {
            Pressed = true;
            queuePress = false;
        }
        else
        {
            Pressed = false;
        }

        if (queueRelease)
        {
            Released = true;
            queueRelease = false;
        }
        else
        {
            Released = false;
        }
    }

    public void Press()
    {
        queuePress = true;
        Held = true;
    }
    public void Release()
    {
        queueRelease = true;
        Held = false;
    }

}
[System.Serializable]
public class PlayerKeyStatus : InputKeyStatus
{
    public KeyCode Key;

    public PlayerKeyStatus(KeyCode key)
    {
        Key = key;
    }
    public override void Update()
    {
        Pressed = Input.GetKeyDown(Key);
        Held = Input.GetKey(Key);
        Released = Input.GetKeyUp(Key);
    }
}