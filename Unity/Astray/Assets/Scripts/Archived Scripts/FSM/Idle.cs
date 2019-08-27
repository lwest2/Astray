using UnityEngine;
using FSM;
using UnityEngine.AI;

public class Idle : State<AI>
{
    private static Idle m_instance;

    // Singleton pattern
    private Idle()
    {
        if (m_instance != null)
        {
            return;
        }

        m_instance = this;
    }

    public static Idle Instance
    {
        get
        {
            if (m_instance == null)
            {
                new Idle();
            }

            return m_instance;
        }
    }

    public override void EnterState(AI _obj)
    {
        Debug.Log("Entering Idle state.");
    }

    public override void ExitState(AI _obj)
    {
        Debug.Log("Exiting Idle state.");
    }

    public override void UpdateState(AI _obj)
    {
        _obj.m_navMeshAgent.isStopped = true;
        _obj.m_navMeshAgent.ResetPath();

        // Look around.

        if (_obj.m_enumStates == AI.enumStates.Wander)
        {
            _obj.m_stateMachine.ChangeState(Wander.Instance);
        }
    }
}
