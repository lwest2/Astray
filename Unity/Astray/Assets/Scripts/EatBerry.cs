using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EatBerry : MonoBehaviour
{
    // Keybindings script.
    private KeyBindings _keys;
    // BerryState script.
    private BerryState _berryState_script;
    // Script for displaying messages. 
    private DisplayMessage m_displayMessage_script;
    // Player manager script.
    private Player_Manager _playerManager_script;

    private Camera m_player;

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 10.0f;
    private string m_message = "There are no berries on this bush...";
    private string m_message2 = "I should wait until it rains for the berries to grow.";

    // TextMeshProGUI reference.
    private TextMeshProUGUI m_textMesh;

    // Text to display text on.
    [SerializeField]
    private GameObject m_text;

    private bool m_triggerOnce = true;

    private Random m_random;

    private void Start()
    {
        m_player = GameObject.Find("Main Camera_2").GetComponent<Camera>();

        // Reference to keybindings.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();
        // Reference berrystate script.
        _berryState_script = GameObject.Find("Berry & Shroom spawnpoints").GetComponent<BerryState>();
        // Reference to player manager script.
        _playerManager_script = GameObject.Find("Manager_Player").GetComponent<Player_Manager>();
        // Script for displaying messages.
        m_displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();

    }

    private void Update()
    {
        RaycastHit hit;
       
        // Raycast infront of the player.
        if (Physics.Raycast(m_player.transform.position, m_player.transform.TransformDirection(Vector3.forward), out hit, 2.5f))
        {
            // If a berry bush is hit.
            if (hit.collider.gameObject.name.Contains("Bush_d1_6x6x4_COL_PRIM_CAPSULE"))
            {
                // If the interact button is pressed.
                if (_keys.GetInteractBool())
                {
                    // If berries are active on the bush.
                    if (_berryState_script.CheckBerryActive(hit.collider.gameObject.transform.parent.transform.parent))
                    {
                        // Check what berry is active on the bush.
                        bool isNew = _berryState_script.CheckBerry(hit.collider.gameObject.transform.parent.transform.parent);
                        
                        // If the berry is ripe.
                        if (isNew)
                        {
                            // Restore 20 sanity.
                            _playerManager_script.TakeDamage(-20.0f);

                            m_displayMessage_script.ResetAndStartCoroutine("Just what I needed...", m_time, m_wait_time);
                        }
                        else
                        { 
                            // Increase 10 sanity. 
                            _playerManager_script.TakeDamage(10.0f);
                            
                            m_displayMessage_script.ResetAndStartCoroutine("Those berries didn't taste right...", m_time, m_wait_time);
                        }
                        

                        _berryState_script.ActivateSingleBerry(hit.collider.gameObject.transform.parent.transform.parent, false);
                    }
                    else
                    {
                        Debug.Log("Object has no berries");
                        // Start coroutine with parameters, notify player on screen.
                        m_time = 0.5f;
                        m_wait_time = 10.0f;

                        // Display message telling the player there are no berries on the bush.
                        int randomChance = Random.Range(0, 2);
                        if (randomChance == 0)
                        {
                            m_displayMessage_script.ResetAndStartCoroutine(m_message, m_time, m_wait_time);
                        } else
                        {
                            m_displayMessage_script.ResetAndStartCoroutine(m_message2, m_time, m_wait_time);
                        }
                    }
                }
            }
        }
    }
}
