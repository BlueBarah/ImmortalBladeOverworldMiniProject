using System;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class BaseStateMachine : MonoBehaviour
    {

        [SerializeField] private BaseState _initialState;

        [field: SerializeField] public BaseState CurrentState { get; set; }

        public NPC NPC;
        public Sensor sensor;

        private Dictionary<Type, Component> _cachedComponents;

        private void Awake()
        {
            _cachedComponents = new Dictionary<Type, Component>();

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

        //For translation movements and anything other than built in physics/rigidbody movements
        private void Update()
        {
            CurrentState.Execute(this);
        }

        //For rigidbody/built in physics movements only
        private void FixedUpdate()
        {
            CurrentState.FixedExecute(this);
        }

        public new T GetComponent<T>() where T : Component
        {
            if (_cachedComponents.ContainsKey(typeof(T)))
                return _cachedComponents[typeof(T)] as T;

            var component = base.GetComponent<T>();
            if (component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }



    }
}