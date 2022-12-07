using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    //Handles collisions for Jason when he runs into an Enemy
    override protected void collisionHandling(GameObject hitObject)
    {
        if (hitObject.CompareTag("Enemy"))
        {
            Debug.Log("BATTLE COMMENCE"); //maybe switch scene or something here?
        }
    }

    //Grabs and returns inputs
    //TODO: configurable inputs (using either keyboard or controller to move)
    private Vector3 getInputDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0, z);
        
        return direction;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = getInputDirection();
        Move(direction);
    }
}
