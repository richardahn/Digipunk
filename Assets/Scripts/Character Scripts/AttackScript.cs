using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public CharacterScript character;
    private float speed = 20.0f;
    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        if (character.isPlayer)
            direction = new Vector3(1f, 0f);
        else
            direction = new Vector3(-1f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (direction * speed * Time.deltaTime);
        // Delete after 5 seconds or after collision
    }
}
