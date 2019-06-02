using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterInput
{
    #region Singleton Implementation
    private static PlayerInput instance;
    public static PlayerInput Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if (instance != null)
            {
                Debug.LogError("There cannot be more than one PlayerInput instance.");
            }
            else
            {
                instance = value;
            }
        }
    }
    #endregion

    // Parameters
    public string horizontalAxis = "Horizontal";
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode interactKey = KeyCode.F;

    public PlayerAbilityList playerAbilityList = new PlayerAbilityList();


    // Lifecycle
    private void Awake()
    {
        Instance = this;
        playerAbilityList.Generate(this.gameObject);
    }

    private void Start()
    {
        Horizontal = new PlayerAxisStatus(horizontalAxis);
        Mouse = new PlayerMouseStatus();
        Interact = new PlayerKeyStatus(interactKey);
        Jump = new PlayerKeyStatus(jumpKey);
    }

    // If I really wanted true segregation, I could implement an IUpdateable interface and add it to a GameEngine monobehaviour that updates all IUpdateables
    protected override void PostUpdate()
    {
        playerAbilityList.Update();
    }

}
