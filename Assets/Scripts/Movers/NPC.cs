using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC : Mover
{
    [SerializeField] public AI myAI; //Every NPC will need some sort of AI (will vary between different types of NPC)
    [field: SerializeField] public virtual Mover target { get; protected set; }
    [SerializeField] public virtual Vector3 targetPosition
    {
        get { return target.currentPosition; }
    }

    public Vector3 nextPosition { get; set; }
    public RaycastHit lastColliderHit { get; protected set; }

    [field: SerializeField] public float eyeLineHeight { get; set; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myAI = this.GetComponent<AI>();
        nextPosition = startingPosition;
        eyeLineHeight = (coll.size.y)/2;
    }

    //Just a visual indicator to show certain behaviors are working
    public void flashColorIndicator(string indicatorString)
    {
        switch (indicatorString)
        {
            case "Obstacle":
                // Turn Blue as visual indicator
                sprite.color = Color.blue;
                break;
            case "Player":
                // Turn red as visual indicator
                sprite.color = Color.red;
                break;
            case "Following":
                //Turn yellow when Enemy chasing
                sprite.color = Color.yellow;
                break;
            case "Moving":
                sprite.color = Color.green;
                break;
            case "Waiting":
                sprite.color = Color.white;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        myAI.calculateAI();
    }
}
