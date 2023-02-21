using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overworld {
public class EnemyEnergyWaveHandler : MonoBehaviour
{
    Enemy owner;
    Animator animator;
    private bool Flag_BattleStart = false;
    // Start is called before the first frame update
    void Awake()
    {
        owner = transform.parent.GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (owner.Flag_BattleStart && !this.Flag_BattleStart) {
            this.Flag_BattleStart = true;
            animator.SetTrigger("Battle Start");
            Debug.Log("Touch");
        }
    }
}

}
