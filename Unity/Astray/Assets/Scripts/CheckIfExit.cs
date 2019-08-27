using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Checks if the player can exit to the village, is deprecated but some of the script is stil in use such as the dialogue.
public class CheckIfExit : MonoBehaviour
{
    // Toggle if the player can exit or not.
    private bool m_canExit = false;

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 10.0f;
    private string m_message = "It's too dangerous to leave the village...";

    // Collider for blocking exit.
    [SerializeField]
    private GameObject m_blockExit;

    // Script for displaying messages. 
    private DisplayMessage m_displayMessage_script;

    private void Start()
    {

        // Script for displaying messages.
        m_displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player enters trigger event.
        if (other.CompareTag("Player"))
        {
            
            // Start coroutine with parameters, notify player on screen.
            m_displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
            
            
            // If able to exit.
            if (m_canExit)
            { 
                // Set block collision to false.
                m_blockExit.SetActive(false);
            }
        }
    }

    // Setter method for exit boolean.
    public void SetCanExit(bool value)
    {
        m_canExit = value;
    }
}
