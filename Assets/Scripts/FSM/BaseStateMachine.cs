using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{

    [SerializeField] private BaseState _initialState;

    [field: SerializeField] public BaseState CurrentState { get; set; }

    public NPC NPC;
    public Sensor sensor;

    private void Awake()
    {
        CurrentState = _initialState;
        NPC = GetComponent<NPC>();
        sensor = GetComponent<Sensor>();

        CurrentState.OnEnter(this);
    }

    public void ChangeCurrentState(BaseState newState)
    {
        if (newState is RemainInState)
        {
            return;
        }

        CurrentState.OnExit(this);
        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    private void Update()
    {
        CurrentState.Execute(this);
    }
}
