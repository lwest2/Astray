using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Toggle crouch for the player.

public class Player_Crouch : MonoBehaviour
{
    // Script to reference key bindings.
    private KeyBindings _keys;

    // Booleans used for toggling crouch.
    private bool m_crouchBool;
    private bool m_crouchToggle = false;

    // GameObject of the player.
    [SerializeField]
    private GameObject m_player_1;

    // Character controller of the player. 
    private CharacterController m_controller_1;

    // Camera 1 and 2, can be switched between, and therefore the crouching will need to act upon both.
    [SerializeField]
    private Camera m_camera_1;
    [SerializeField]
    private Camera m_camera_2;

    // Start and end position for the camera when toggling crouch.
    [SerializeField]
    private Transform m_startPosition_1;
    [SerializeField]
    private Transform m_endPosition_1;


    private void Start()
    {
        // Reference for keybindings.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();
        
        // Reference for character controller.
        m_controller_1 = m_player_1.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get inputs.
        GetInputs();

        // Perform crouch.
        Crouch();
    }

    public void GetInputs()
    {
        // Assign crouch boolean that is dependent on if a button is pressed within keybindings.
        m_crouchBool = _keys.GetCrouchBool();
    }

    public void Crouch()
    {
        // Variables to perform raycast.
        RaycastHit hit;
        Vector3 p1;

        // p1 is equal to the players position plus the character controller center. Therefore just above the player.
        p1 = m_player_1.transform.position + m_controller_1.center;
        
        // Cast a sphere upwards.
        if (Physics.SphereCast(p1, m_controller_1.radius, Vector3.up, out hit, 1f))
        {
            // if crouched input, toggle crouch
            if (m_crouchBool)
            {
                m_crouchToggle = true;
            }
        }
        else
        {
            // If cast did not hit, but the key is pressed.
            if (m_crouchBool)
            {
                // Toggle crouch.
                m_crouchToggle = !m_crouchToggle;
            }
        }

        // if crouched toggle
        if (m_crouchToggle)
        {
            // Move camera downwards towards end position.
            m_camera_1.transform.position = Vector3.Lerp(m_camera_1.transform.position, m_endPosition_1.position, Time.deltaTime * 2);
            m_camera_2.transform.position = Vector3.Lerp(m_camera_1.transform.position, m_endPosition_1.position, Time.deltaTime * 2);

            // if Character controller height is more than 1.61.
            if (m_controller_1.height > 1.61f)
            {
                // Lerp player height towards 1.6. (Crouch)
                m_controller_1.height = Mathf.Lerp(m_controller_1.height, 1.6f, Time.deltaTime * 2);
            }
            else
            {
                // If within range, snap to 1.6 to stop lerp.
                m_controller_1.height = 1.6f;
            }
        }
        else
        {

            // Move camera downwards towards start position.
            m_camera_1.transform.position = Vector3.Lerp(m_camera_1.transform.position, m_startPosition_1.position, Time.deltaTime * 2);
            m_camera_2.transform.position = Vector3.Lerp(m_camera_1.transform.position, m_startPosition_1.position, Time.deltaTime * 2);

            // if Character controller height is less than 2.09.
            if (m_controller_1.height < 2.09f)
            {
                // Lerp player height towards 2.1.
                m_controller_1.height = Mathf.Lerp(m_controller_1.height, 2.1f, Time.deltaTime * 2);

                // Keep player just above the ground in order to prevent jitter.
                m_player_1.transform.position = new Vector3(m_player_1.transform.position.x, m_player_1.transform.position.y + Time.deltaTime / 2, m_player_1.transform.position.z);
            }
            else
            {
                // If within range, snap to 2.1 to stop lerp.
                m_controller_1.height = 2.1f;
            }

        }
    }

    // Getter for crouch toggle.
    public bool GetCrouchToggle()
    {
        return m_crouchToggle;
    }
}
