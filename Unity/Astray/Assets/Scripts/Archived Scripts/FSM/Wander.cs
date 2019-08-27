using UnityEngine;
using FSM;
using UnityEngine.AI;

public class Wander : State<AI>
{
    private static Wander m_instance;
    private bool chooseDir = true;

    // Singleton pattern
    private Wander()
    {
        if (m_instance != null)
        {
            return;
        }

        m_instance = this;
    }

    public static Wander Instance
    {
        get
        {
            if (m_instance == null)
            {
                new Wander();
            }

            return m_instance;
        }
    }

    public override void EnterState(AI _obj)
    {
        Debug.Log("Entering Wander state.");
    }

    public override void ExitState(AI _obj)
    {
        Debug.Log("Exiting Wander state.");
    }

    public override void UpdateState(AI _obj)
    {
        // Reference: https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/

        if (chooseDir)
        {
            chooseDir = false;

            Vector3 randomDir = Random.insideUnitSphere * 100.0f;
            randomDir += _obj.transform.position;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDir, out navHit, 100.0f, -1);
            Vector3 newPos = navHit.position;

            _obj.m_navMeshAgent.SetDestination(newPos);
        }

        if (_obj.m_enumStates == AI.enumStates.Idle)
        {
            chooseDir = true;
            _obj.m_stateMachine.ChangeState(Idle.Instance);
        }
    }
}
