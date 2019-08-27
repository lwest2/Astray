using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM;

public class AI : MonoBehaviour
{
    private float m_gameTimer;
    private int m_seconds;
    private int m_randomNumber;
    private bool m_pickRandomNumber = true;

    public NavMeshAgent m_navMeshAgent;

    public enum enumStates
    {
        Idle,
        Wander,
        Converse,
    }

    public enumStates m_enumStates;
    private int _randomNumber = 0;

    public StateMachine<AI> m_stateMachine { get; set; }

    private void Start()
    {
        m_stateMachine = new StateMachine<AI>(this);
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_stateMachine.ChangeState(Idle.Instance);
        m_gameTimer = Time.time;
    }

    private void Update()
    {
        // Choose state.

        // If idle for 5 - 10 seconds. Wander.
        if (Time.time > m_gameTimer + 1)
        {
            m_gameTimer = Time.time;
            m_seconds++;

            // Pick a number from 5 - 10.
            if (m_pickRandomNumber)
            {
                m_pickRandomNumber = false;
                m_randomNumber = RandomNumber(5, 10);
            }
        }

        if (m_enumStates == enumStates.Idle)
        {
            if (m_seconds == m_randomNumber)
            {
                m_seconds = 0;
                m_enumStates = enumStates.Wander;
                m_pickRandomNumber = true;
            }
        }

        // If wandering for 5 - 10 seconds. Idle.
        if (m_enumStates == enumStates.Wander)
        {
            if (m_seconds == m_randomNumber)
            {
                m_seconds = 0;
                m_enumStates = enumStates.Idle;
                m_pickRandomNumber = true;
            }
        }

        m_stateMachine.Update();
    }

    private int RandomNumber(int min, int max)
    {
        _randomNumber = Random.Range(min, max);

        return _randomNumber;
    }
}
