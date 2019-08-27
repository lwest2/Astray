using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private Camera m_player;
    private bool m_triggerOnce = true;
    private const string m_berryBush = "Bush_d1_6x6x4_COL_PRIM_CAPSULE";
    private const string m_woodPile = "woodpile";
    private const string m_fire = "fire_stonemound";

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 5.0f;

    // Script for displaying messages. 
    private DisplayMessage m_displayMessage_script;

    private void Start()
    {
        m_player = GameObject.Find("Main Camera_2").GetComponent<Camera>();

        // Script for displaying messages.
        m_displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();
    }
    
    void Update()
    {
        // Raycast detection for each object.
        Raycast(m_berryBush);
        Raycast(m_woodPile);
        Raycast(m_fire);
    }

    private void Raycast(string obj)
    {
        RaycastHit hit;
        
        // If raycast out is hit.
        if (Physics.Raycast(m_player.transform.position, m_player.transform.TransformDirection(Vector3.forward), out hit, 2.5f))
        {
            if (hit.collider.gameObject.name.Contains(obj))
            {
                if (m_triggerOnce)
                {
                    // Display message for specific object.
                    m_triggerOnce = false;
                    StartCoroutine(DisplayRaycastText(obj));
                }
            }
        }
    }

    private IEnumerator DisplayRaycastText(string obj)
    {
        Debug.Log("displaying");
        string message = "Nothing to do here...";

        // Switch message depending on objects.
        switch (obj)
        {
            case m_berryBush:
                message = "Press [E] to check for berries.";
                break;
            case m_fire:
                message = "Press [E] to light fire with wood.";
                break;
            case m_woodPile:
                message = "Press [E] to collect wood from woodpile.";
                break;
            case null:
                message = "Nothing to do here...";
                break;
        }

        m_displayMessage_script.ResetAndStartCoroutine(message, m_time, m_wait_time);

        yield return new WaitForSeconds(6.0f);

        m_triggerOnce = true;

        yield return null;
    }
}
