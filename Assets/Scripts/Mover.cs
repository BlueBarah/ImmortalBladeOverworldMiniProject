using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 8; //defualt speed for now
    [SerializeField] float ySpeed; //usually 0, could be useful for flying enemies later tho
    protected SpriteRenderer sprite;

    // Animation Conditions
    private Animator animator;
    public bool isRunning { get; private set;}

    //TODO:
    //-LOS cone for detection (probably based on movement direction)
    //-collision box/range for battle engagement


    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isRunning = false;
    }

    //Move in the direction given, also flips sprite when moving to the left
    //TODO: maybe change movement system to rb.movePosition
    protected void Move(Vector3 direction)
    {
        // Animation Conditions
        if (direction != Vector3.zero) {
            isRunning = true;
        }
        else {
            isRunning = false;
        }

        if (animator != null) {
            animator.SetBool("isRunning", isRunning);
        }
        else {
            animator = GetComponent<Animator>();
        }

        Vector3 moveDelta = new Vector3(direction.x * walkingSpeed, direction.y * ySpeed, direction.z * walkingSpeed);

        //Mover is moving to right, unflip sprite
        if(direction.x > 0)
        {
            sprite.flipX = false;
        }else if(direction.x < 0) // Moving to left, flip sprite
        {
            sprite.flipX = true;
        }

        transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, moveDelta.z * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
