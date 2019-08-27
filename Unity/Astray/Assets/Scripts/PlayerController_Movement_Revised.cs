using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move player at different speeds depending on key pressed.

public class PlayerController_Movement_Revised : MonoBehaviour
{
    // Keybindings script.
    private KeyBindings _keys;

    // Speed types and their variables.
    [SerializeField]
    private float m_walkSpeed = 3.0f;
    [SerializeField]
    private float m_runSpeed = 3.7f;
    [SerializeField]
    private float m_sneakSpeed = 1.5f;

    // Jump speed.
    [SerializeField]
    private float m_jumpSpeed = 3.0f;

    // Chosen speed variable.
    private float m_chosenSpeed = 0.0f;

    // Vectors associated with direction of movement and clamped vectors.
    private Vector3 m_moveDirection = Vector3.zero;
    private Vector2 m_clampedVector = Vector3.zero;

    // Character controller.
    private CharacterController m_controller;

    // Crouch toggle.
    private bool m_crouchToggle = false;

    // Gravity.
    [SerializeField]
    private float m_gravity = 20.0f;

    // Inputs.
    private float m_horizontalAxis;
    private float m_verticalAxis;
    private bool m_runBool;
    private bool m_sneakBool;
    private bool m_jumpBool;
    private bool m_crouchBool;

    // Transform.
    private Transform m_transform;

    // Both cameras.
    [SerializeField]
    private Camera m_camera_1;
    [SerializeField]
    private Camera m_camera_2;

    private AudioSource m_footsteps;

    private void Start()
    {
        // Reference to character controller.
        m_controller = GetComponent<CharacterController>();

        // Reference to transform.
        m_transform = GetComponent<Transform>();
        
        // Reference to keybindings.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();

        m_footsteps = GameObject.Find("Footsteps").GetComponent<AudioSource>();
        m_footsteps.Play();
        m_footsteps.Pause();
    }

    private void Update()
    {
        // Firstly, get inputs.
        GetInputs();

        // Next, set speed.
        SetSpeed();

        // Change field of view of cameras based on speeds.
        FieldOfView(m_camera_1);
        FieldOfView(m_camera_2);

        // If player is grounded.
        if (m_controller.isGrounded)
        {
            // Move.
            Movement();
        } else
        {
            // Apply jump.
            Jump();
        }

        PlaySound();

        // Apply to rigidbody / character controller.
        ApplyMovement();
    }

    public void GetInputs()
    {
        // Get inputs from keybindings script.
        m_horizontalAxis = _keys.GetHorizontalAxis();
        m_verticalAxis = _keys.GetVerticalAxis();
        m_runBool = _keys.GetRunBool();
        m_sneakBool = _keys.GetSneakBool();
        m_jumpBool = _keys.GetJumpBool();
    }

    public void SetSpeed()
    {
        if (m_runBool && m_sneakBool)                                               // if running and sneaking.
        {
            // Set speed to walkspeed.
            m_chosenSpeed = m_walkSpeed;
        } 
        else if ((m_runBool && m_sneakBool && m_crouchToggle) || (m_crouchToggle))  // if running, sneaking and crouching, or just crouching.
        {
            // Set speed to sneakspeed.
            m_chosenSpeed = m_sneakSpeed;
        }
        else if (m_sneakBool && !m_runBool)                                         // if sneaking but not running.
        {
            // Set speed to sneakspeed.
            m_chosenSpeed = m_sneakSpeed;
        }
        else if (m_runBool && !m_sneakBool)                                         // if running but not sneaking.
        {
            // Set speed to runspeed.
            m_chosenSpeed = m_runSpeed;
        }
        else                                                                        // else walk
        {
            // Set speed to walkspeed.
            m_chosenSpeed = m_walkSpeed;
        }
    }

    public void Movement()
    {
        // Get movement direction based on button axis.
        m_moveDirection = new Vector3(m_horizontalAxis, 0, m_verticalAxis);
        
        // Clamp magnitude to prevent walk straffing.
        m_moveDirection = Vector3.ClampMagnitude(m_moveDirection, 1f);
 
        // Move direction is made the direction.
        m_moveDirection = m_transform.TransformDirection(m_moveDirection);

        // Apply this direction multiplied by the chosen speed.
        m_moveDirection *= m_chosenSpeed;

        // If jumping.
        if (m_jumpBool)
        {
            // Move direction upwards equal to jump speed. 
            m_moveDirection.y = m_jumpSpeed;
        }
    }

    public void Jump()
    {
        // Assigns move direction speed to upwards direction.
        m_moveDirection = new Vector3(m_horizontalAxis, m_moveDirection.y, m_verticalAxis);
        
        // Clamps vector of move direction and magnitude to prevent straffing.
        m_clampedVector = new Vector3(m_moveDirection.x, m_moveDirection.z);
        m_clampedVector = Vector2.ClampMagnitude(m_clampedVector, 1f);
        
        // Assign clamped vector variables.
        m_moveDirection.x = m_clampedVector.x;
        m_moveDirection.z = m_clampedVector.y;

        // Move direction applied.
        m_moveDirection = m_transform.TransformDirection(m_moveDirection);

        // Multiply move direction by chosen speed.
        m_moveDirection.x *= m_chosenSpeed;
        m_moveDirection.z *= m_chosenSpeed;

    }

    private void PlaySound()
    {
        if (m_moveDirection.x != 0 && m_moveDirection.z != 0)
        {
            if (m_chosenSpeed == m_runSpeed)
            {
                m_footsteps.pitch = 1.3f;
                m_footsteps.UnPause();
            }
            else
            {
                m_footsteps.pitch = 1.0f;
                m_footsteps.UnPause();
            }
        }
        else
        {
            m_footsteps.Pause();
        }
    }

    public void ApplyMovement()
    {
        // Decrease value by gravity overtime.
        m_moveDirection.y -= m_gravity * Time.deltaTime;
        
        // Move character controller based on inputs over time.
        m_controller.Move(m_moveDirection * Time.deltaTime);
    }

    public void FieldOfView(Camera camera)
    {
        // If chosen speed is runSpeed.
        if (m_chosenSpeed == m_runSpeed)
        {
            // Lerp field of view to become 67.
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 67.0f, Time.deltaTime * 3.0f);
        }
        else
        {
            // Lerp field of view towards 60.       
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60.0f, Time.deltaTime * 5.0f);

            // if below 60.1
            if (camera.fieldOfView < 60.1f)
            {
                // Snap camera FOV to 60.0.
                camera.fieldOfView = 60.0f;
            }
        }
    }

}

