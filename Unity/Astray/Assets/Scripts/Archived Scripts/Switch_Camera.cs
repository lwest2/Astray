using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Camera : MonoBehaviour
{
    // Variables
    private KeyBindings _keys;

    // Input for view
    private bool m_ToggleView = false;
    private bool m_ToggleBool = false;

    [SerializeField]
    private Camera m_camera_1;

    [SerializeField]
    private Camera m_camera_2;

    private bool applyOnce = true;
    private bool applyOnce_2 = true;

    private GameObject[] m_temp_all;
    private List<GameObject> m_PsychosisObjects_list = new List<GameObject>();
    private List<GameObject> m_NormalObjects_list = new List<GameObject>();

    private void Start()
    {
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();

        FindGameObjects();
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

    void FindGameObjects()
    {
        m_temp_all = FindObjectsOfType<GameObject>();
        
        foreach(GameObject obj in m_temp_all)
        {
            if (obj.layer == 8)
            {
                m_PsychosisObjects_list.Add(obj);
                Debug.Log(obj + " assigned to PsychosisObjects");
            } else if (obj.layer == 9)
            {
                m_NormalObjects_list.Add(obj);
                Debug.Log(obj + " assigned to NormalObjects");
            }
        }
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
        // Sees psychosis objects
        if (m_ToggleBool)
        {
            applyOnce_2 = true;

            // Get Vectors from start point (Do once)
            if (applyOnce)
            {
                // Switching to Camera 2
                applyOnce = false;

                // Seamless turn on and off the different cameras 
                EnableCameraTwo();
            }
        }
        else
        {
            applyOnce = true;

            if (applyOnce_2)
            {
                // Switching to player 1.
                applyOnce_2 = false;

                // Seamless turn on and off the different cameras 
                EnableCameraOne();
            }
        }
    }

    void EnableCameraOne()
    {
        // Seamless turn on and off the different cameras 
        m_camera_2.enabled = true;
        m_camera_1.enabled = false;

        // Disable normal psychosis colliders
        foreach (GameObject obj in m_PsychosisObjects_list)
        {
            if (obj.GetComponent<Collider>().enabled)
            {
                obj.GetComponent<Collider>().enabled = false;
            }
        }

        foreach (GameObject obj in m_NormalObjects_list)
        {
            if (!obj.GetComponent<Collider>().enabled)
            {
                obj.GetComponent<Collider>().enabled = true;
            }
        }
    }

    void EnableCameraTwo()
    {
        m_camera_1.enabled = true;
        m_camera_2.enabled = false;

        // Disable normal objects colliders
        foreach (GameObject obj in m_NormalObjects_list)
        {
            if (obj.GetComponent<Collider>().enabled)
            {
                obj.GetComponent<Collider>().enabled = false;
            }
        }

        foreach (GameObject obj in m_PsychosisObjects_list)
        {
            if (!obj.GetComponent<Collider>().enabled)
            {
                obj.GetComponent<Collider>().enabled = true;
            }
        }
    }
}
