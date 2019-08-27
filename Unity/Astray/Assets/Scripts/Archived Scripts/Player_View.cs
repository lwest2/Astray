using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_View : MonoBehaviour
{
    // Variables
    private KeyBindings _keys;

    // Input for view
    private bool m_ToggleView = false;
    private bool m_ToggleBool = false;

    [SerializeField]
    private GameObject m_player_1;  
    [SerializeField]
    private GameObject m_player_2;

    // Both start positions.
    [SerializeField]
    private GameObject m_startPosition_1;
    [SerializeField]
    private GameObject m_startPosition_2;

    // Both cameras.
    [SerializeField]
    private Camera m_camera_1;
    [SerializeField]
    private Camera m_camera_2;

    // Booleans used so that methods are not called twice, or if statements are not entered twice in Update.
    private bool applyOnce = true;
    private bool applyOnce_2 = true;

    private void Start()
    {
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();
    }

    private void Update()
    {
        // Inputs
        GetInputs();

        // Toggle
        Toggle();

        // Switch view
        Switch();
    }

    void GetInputs()
    {
        m_ToggleView = _keys.GetViewBool();
    }

    void Toggle()
    {
        if (m_ToggleView)
        {
            m_ToggleBool = !m_ToggleBool;
        }
    }

    void Switch()
    {
        // if toggle false, play as 2
        if (!m_ToggleBool)
        {
            applyOnce_2 = true;

            // Get Vectors from start point (Do once)
            if (applyOnce)
            {
                // Switching to player 2.

                applyOnce = false;

                Vector3 pos = m_player_1.transform.position - m_startPosition_1.transform.position;

                m_player_2.transform.position = m_startPosition_2.transform.position + pos;

                // Seamless turn on and off the different cameras 
                m_player_2.SetActive(true);
                m_player_1.SetActive(false);
               
            }
        }
        else
        {
            applyOnce = true;

            if (applyOnce_2)
            {
                // Switching to player 1.
                applyOnce_2 = false;

                Vector3 pos = m_player_2.transform.position - m_startPosition_2.transform.position;

                m_player_1.transform.position = m_startPosition_1.transform.position + pos;

                Quaternion rot = m_player_1.transform.rotation;
                Debug.Log("Player 2 rot: " + rot);

                // Seamlessly turn on and off the different cameras
                m_player_1.SetActive(true);
                m_player_2.SetActive(false);
            }
        }
    }

}
