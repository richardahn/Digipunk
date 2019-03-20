using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : CharacterInput
{
    
    KeyCode currentHeldKey = KeyCode.None;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // While locked from keypresses(such as during a stun, or after u use a heavy attack, you cant perform any actions), be able to queue up things
    // Or while casting or something

    // Update is called once per frame
    void Update()
    {
        // Check charge keys
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Map from key to ability command and send it to PlayerManager
            /*
             * TODO: Make notify return bool for success/failure. If it failed, queue that key up? and once the animation frees up, make it perform the command?
             * */
            character.actions.Notify(CharacterScript.CHARGE);
            currentHeldKey = KeyCode.Q;
        }

        // Check if charge keys were released
        if (Input.GetKeyUp(currentHeldKey))
        {
            character.actions.Notify(CharacterScript.RELEASE);
            currentHeldKey = KeyCode.None;
        }
    }
}
