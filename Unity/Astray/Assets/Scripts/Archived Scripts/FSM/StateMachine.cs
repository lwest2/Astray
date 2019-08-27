using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

    // Reference: https://www.youtube.com/watch?v=PaLD1t-kIwM&t=749s

    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; } // current state of object.
        public T obj; // AI object that states are in effect on.

        // constructor
        public StateMachine(T _obj)
        {
            obj = _obj;
            currentState = null;
        }

        public void ChangeState(State<T> _newState)
        {
            if (currentState != null)
            {
                currentState.ExitState(obj);
            }

            currentState = _newState;
            currentState.EnterState(obj);
        }

        public void Update()
        {
           
            if (currentState != null)
            {
                currentState.UpdateState(obj);
            }
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T _obj);
        public abstract void ExitState(T _obj);
        public abstract void UpdateState(T _obj);
    }
}
