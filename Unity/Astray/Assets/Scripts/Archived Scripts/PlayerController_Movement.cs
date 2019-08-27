using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Movement : MonoBehaviour
{
    // Variables
    private KeyBindings _keys;

    // Different speed variations
    [SerializeField]
    private float m_walkSpeed = 25.0f;

    [SerializeField]
    private float m_runSpeed = 45.0f;

    [SerializeField]
    private float m_sneakSpeed = 12.5f;

    private float m_chosenSpeed = 0.0f;

    // Inputs
    private float m_horizontalAxis;
    private float m_verticalAxis;
    private bool m_runBool;
    private bool m_sneakBool;
    private bool m_jumpBool;
    private bool m_crouchBool;

    // Jump height
    [SerializeField]
    private float m_jumpHeight = 6.0f;

    // Crouch toggle
    private bool m_crouchToggle = false;

    // Rigidbody needed for physics calculations
    private Rigidbody m_rigidbody;

    // Transform
    private Transform m_transform;

    // velocity vector
    private Vector3 m_velocity;

    // Camera
    [SerializeField]
    private Camera m_camera = null;

    [SerializeField]
    private Transform m_cameraStart = null;

    [SerializeField]
    private Transform m_cameraEnd = null;

    // Crouch colliders
    [SerializeField]
    private GameObject m_standCollider = null;

    [SerializeField]
    private GameObject m_crouchCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_transform = GetComponent<Transform>();
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();
    }

    // Update is called once per frame
    void Update()
    {
        // Inputs
        GetInputs();

        // Chooses direction based on inputs
        SetDirection();

        // Chooses speed depending on players input
        SetSpeed();

        // Toggle Crouch
        Crouch();
    }

    private void FixedUpdate()
    {
        // if player is grounded
        if (isGrounded() && !m_crouchToggle)
        {
            // if jump activated and is grounded
            if (m_jumpBool)
            {
                // Add force to player in the up direction
                m_rigidbody.AddForce(Vector3.up * m_jumpHeight, ForceMode.Impulse);
            }

            // if grounded choose normal speed
            movePlayer(m_chosenSpeed);
        }
        else
        {
            // else if slow player in air
            movePlayer(m_chosenSpeed);
        }
    }

    public void GetInputs()
    {
        m_horizontalAxis = _keys.GetHorizontalAxis();
        m_verticalAxis = _keys.GetVerticalAxis();
        m_runBool = _keys.GetRunBool();
        m_sneakBool = _keys.GetSneakBool();
        m_jumpBool = _keys.GetJumpBool();
        m_crouchBool = _keys.GetCrouchBool();
    }

    public void SetDirection()
    {
        // save movements to a vector
        m_velocity = new Vector3(m_horizontalAxis, 0, m_verticalAxis);

        // Prevents diagonal speed increase
        m_velocity = Vector3.ClampMagnitude(m_velocity, 1f);
    }

    public void SetSpeed()
    {
        // Set speed based on inputs
        if (m_runBool && m_sneakBool)
        {
            m_chosenSpeed = m_walkSpeed;
        }
        else if (m_runBool && isGrounded())
        {
            m_chosenSpeed = m_runSpeed;
        }
        else if ((m_sneakBool || m_crouchToggle) && isGrounded())
        {
            m_chosenSpeed = m_sneakSpeed;
        }
        else
        {
            m_chosenSpeed = m_walkSpeed;
        }
    }

    public void Crouch()
    {
        // if crouched input, toggle crouch
        if (m_crouchBool)
        {
            m_crouchToggle = !m_crouchToggle;
        }

        // if crouched toggle
        if (m_crouchToggle)
        {
            //  Move camera downwards and change collider
            m_camera.transform.position = Vector3.MoveTowards(m_camera.transform.position, m_cameraEnd.position, Time.deltaTime);
            m_crouchCollider.SetActive(true);
            m_standCollider.SetActive(false);
        }
        else
        {
            // Move camera back to default position and change collider
            m_camera.transform.position = Vector3.MoveTowards(m_camera.transform.position, m_cameraStart.position, Time.deltaTime);
            m_standCollider.SetActive(true);
            m_crouchCollider.SetActive(false);
        }
    }

    public void movePlayer(float _speed)
    {
        // Move player in horizontal and vertical directions
        m_rigidbody.MovePosition(m_rigidbody.position + (transform.forward * m_velocity.z) * _speed * Time.fixedDeltaTime);
        m_rigidbody.MovePosition(m_rigidbody.position + (transform.right * m_velocity.x) * _speed * Time.fixedDeltaTime);
    }

    public bool isGrounded()
    {
        // if position of player is within specified distance to layer mask 'ground' return true
        if (Physics.Raycast((new Vector3(m_transform.position.x, m_transform.position.y + 1f, m_transform.position.z)), -Vector3.up, 2f, 1 << LayerMask.NameToLayer("Ground")))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
