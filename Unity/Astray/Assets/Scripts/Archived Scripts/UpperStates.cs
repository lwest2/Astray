using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperStates : MonoBehaviour
{
    private float m_hunger = 0.0f;
    private float m_thirst = 0.0f;
    private float m_sanity = 100.0f;

    private bool m_decreaseSanity = false;
    private StateBehaviour m_stateBehavior_script;

    private float m_anxietyMulti = 1.0f;
    private float m_depressionMulti = 1.2f;
    private float m_locMulti = 0.2f;

    private void Start()
    {
        m_stateBehavior_script = GameObject.Find("States").GetComponent<StateBehaviour>();
        InvokeRepeating("DecreaseSanity", 0.0f, 5.0f);
        InvokeRepeating("IncreaseHunger", 0.0f, 3.0f);
        InvokeRepeating("IncreaseDehydration", 0.0f, 3.0f);
    }

    public void IncreaseHunger()
    {
        // Increase hunger if below 100.
        if (m_hunger < 100.0f)
        {
            m_hunger += 0.5f;

            if (m_hunger > 100.0f)
            {
                m_hunger = 100.0f;
            }
        }
    }

    public void IncreaseDehydration()
    {
        // Increase thirst if below 100.
        if (m_thirst < 100.0f)
        {
            m_thirst += 0.5f;

            if (m_thirst > 100.0f)
            {
                m_thirst = 100.0f;
            }
        }
    }


    public void DecreaseSanity()
    {
        // Increase sanity value.
        // Therefore increase anxiety / depression / lack of concentration.

        if (m_decreaseSanity)
        {
            // Decrease sanity if more than 0.
            if (m_sanity > 0.0f)
            {
                // Sanity decreases by default. (Acts as a timer for the player per level.)
                m_sanity -= 1.0f;

                // If above 100 then set to 100.
                if (m_sanity < 0.0f)
                {
                    m_sanity = 0.0f;
                }

                Debug.Log("Sanity: " + m_sanity + ", Hunger: " + m_hunger + ", Thirst: " + m_thirst);
            }
            

            float inversedSanity = 100.0f - m_sanity;
            m_stateBehavior_script.m_p1Anxiety = (inversedSanity / 100.0f) * m_anxietyMulti;
            m_stateBehavior_script.m_p1LOConcentration = (inversedSanity / 100.0f) * m_locMulti;
            m_stateBehavior_script.m_p1Depression = (inversedSanity / 100.0f) * m_depressionMulti;
        }
    }

    void SetIncreaseValues(bool value)
    {
        m_decreaseSanity = value;
    }
}
