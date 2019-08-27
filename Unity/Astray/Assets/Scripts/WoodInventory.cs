using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When wood is taken from the wood pile, add to inventory and deplete woodpile amount.

public class WoodInventory : MonoBehaviour
{
    private int m_woodInventory = 0;
    private int m_maxWoodPile = 3;
    private Camera m_player;

    // Script for displaying messages. 
    private DisplayMessage m_displayMessage_script;
    // Keybindings script.
    private KeyBindings _keys;

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 10.0f;
    private string m_message = null;

    private void Start()
    {
        m_player = GameObject.Find("Main Camera_2").GetComponent<Camera>();

        // Script for displaying messages.
        m_displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();
        // Reference to keybindings.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();

    }

    void Update()
    {
        RaycastHit hit;

        // If woodpile is hit by raycast and interact button is pressed.
        if (Physics.Raycast(m_player.transform.position, m_player.transform.TransformDirection(Vector3.forward), out hit, 2.5f))
        {
            if (hit.collider.gameObject.name.Contains("woodpile"))
            {
                if (_keys.GetInteractBool())
                {
                    // if wood inventory is 0.
                    if (m_woodInventory == 0)
                    {
                        // take wood and deplete wood pile by 1.
                        m_woodInventory = 1;
                        m_maxWoodPile--;

                        m_message = "This looks good enough to put on the fire...";
                        m_displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);

                        // if wood pile is empty. 
                        if (m_maxWoodPile == 0)
                        {
                                m_message = "The rest of the woodpile looks too damp to be burnt.";
                                m_displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
                        }
                    } else if (m_woodInventory == 1) {
                        // If inventory of wood is already 1.
                        m_message = "I should probably burn the wood I already have.";
                        m_displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
                    }
                }
            }
        }
    }

    public int GetWoodInventory()
    {
        return m_woodInventory;
    }
}
