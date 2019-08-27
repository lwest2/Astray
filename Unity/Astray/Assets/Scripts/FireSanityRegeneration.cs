using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Staying close to the fire replenishes sanity.

public class FireSanityRegeneration : MonoBehaviour
{
    private ToggleFire _toggleFire_script;
    private Player_Manager _playerManager_script;
    // Display message script.
    private DisplayMessage _displayMessage_script;

    private bool m_triggerOnce = true;

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 10.0f;
    private string m_message = null;

    private void Start()
    {
        _toggleFire_script = GameObject.Find("fire_stonemound").GetComponent<ToggleFire>();
        _playerManager_script = GameObject.Find("Manager_Player").GetComponent<Player_Manager>();

        // Script for displaying messages.
        _displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();

    }

    private void OnTriggerStay(Collider other)
    {
        // If player is near fire and the fire is on.
        if (other.name == "Player" && _toggleFire_script.GetFireOn())
        {
            if (m_triggerOnce)
            {
                // Begin regen.
                m_triggerOnce = false;
                _playerManager_script.StartRegen();
                m_message = "The fire is warm and comforting...";
                _displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
            }    
        }
        else if (other.name == "Player" && !_toggleFire_script.GetFireOn())
        {
            // If the fire is off then stop the regen.
            _playerManager_script.StopRegen();
        }
    }

    // When the player exits the area where the fire is.
    private void OnTriggerExit(Collider other)
    {
        // Stop players regen.
        if (other.name == "Player")
        {
            _playerManager_script.StopRegen();
        }
    }

}
