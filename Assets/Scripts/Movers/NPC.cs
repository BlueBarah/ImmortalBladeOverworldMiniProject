using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Mover
{
    [SerializeField] public AI myAI; //Every NPC will need some sort of AI (will vary between different types of NPC)
    [SerializeField] public Transform target { get; set; }
    public Vector3 nextPosition { get; set; }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        nextPosition = startingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
