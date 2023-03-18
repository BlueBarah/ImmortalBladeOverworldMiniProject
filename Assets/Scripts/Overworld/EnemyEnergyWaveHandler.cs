using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overworld {
public class EnemyEnergyWaveHandler : MonoBehaviour
{
    IDetectPlayer owner;
    Animator animator;
    private bool flag_battleStart = false;
    // Start is called before the first frame update
    void Awake()
    {
        owner = transform.parent.GetComponent<IDetectPlayer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (owner.flag_battleStart && !this.flag_battleStart) {
            this.flag_battleStart = true;
            animator.SetTrigger("Battle Start");
            Debug.Log("Touch");
        }
    }
}

}
