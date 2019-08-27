using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checks if the player has wood in their inventory, if so then they can relight the fire.

public class RelightFire : MonoBehaviour
{ 
    private Camera m_player;

    // Script for displaying messages. 
    private DisplayMessage _displayMessage_script;
    // Keybindings script.
    private KeyBindings _keys;
    // ToggleFire script.
    private ToggleFire _toggleFire_script;
    // WoodInventory script.
    private WoodInventory _woodInventory_script;

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 10.0f;
    private string m_message = null;

    // Rain object.
    private GameObject m_rainObject;
    // Particle system associated with rain.
    private ParticleSystem m_rainSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.Find("Main Camera_2").GetComponent<Camera>();

        // Script for displaying messages.
        _displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();
        // Reference to keybindings.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();
        _toggleFire_script = GameObject.Find("fire_stonemound").GetComponent<ToggleFire>();
        _woodInventory_script = GameObject.Find("Manager_Player").GetComponent<WoodInventory>();

        // Rain Object
        m_rainObject = GameObject.Find("Rain");
        // rainObject particle system.
        m_rainSystem = m_rainObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Must check if fire is out. Gives feedback if already lit.
        // Must check wood collection.

        RaycastHit hit;

        if (Physics.Raycast(m_player.transform.position, m_player.transform.TransformDirection(Vector3.forward), out hit, 2.5f))
        {
            if (hit.collider.gameObject.name.Contains("fire_stonemound"))
            {
                if (_keys.GetInteractBool())
                {
                    if (_toggleFire_script.GetFireOn())
                    {
                        // Display message saying fire is already on.
                        m_message = "There is wood already burning here.";
                        _displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);

                    } else
                    {
                        if (!m_rainSystem.isStopped)
                        {
                            m_message = "No point relighting the fire when its raining...";
                            _displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
                        }
                        else if (_woodInventory_script.GetWoodInventory() > 0)
                        {
                            m_message = "That should keep it alight for a bit.";
                            _displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
                            _toggleFire_script.TurnFireOn();
                        }
                    }
                }
            }
        }
    }
}
