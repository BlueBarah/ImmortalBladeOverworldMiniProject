using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitAI
{
    public Vector3 nextPosition { get; set; }
    public Vector3 startingPosition { get; set; }
    public Vector3 currentPosition { get; set; }
    public Transform target { get; set; }

    public void ManageState();
}
