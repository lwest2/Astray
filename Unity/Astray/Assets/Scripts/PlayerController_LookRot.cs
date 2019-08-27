using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Look rotation for player.

public class PlayerController_LookRot : MonoBehaviour
{
    // Keybindings script.
    private KeyBindings _keys;

    // Sensitivity of mouse movement.
    [SerializeField]
    private float m_sensitivityX = 3.0f;
    [SerializeField]
    private float m_sensitivityY = 3.0f;

    // Rotation input.
    private float m_rotationY = 0f;
    private float m_rotationX = 0f;

    // Current rotation / used for damping.
    private float m_rotationX_current;
    private float m_rotationY_current;

    // Velocity / used for damping.
    private float m_rotationX_velocity;
    private float m_rotationY_velocity;

    // Smooth damping variable.
    [SerializeField]
    private float m_smoothDamp = 0.1f;

    // Cameras and rotation.
    [SerializeField]
    private Camera m_camera_1;
    [SerializeField]
    private Transform m_rotation_1;   
    [SerializeField]
    private Camera m_camera_2;

    // Clamps for X and Y rotation.
    private float m_minYRotation = -60.0f;
    private float m_maxYRotation = 60.0f;

    [SerializeField]
    private GameObject m_pauseObject;

    private void OnEnable()
    {
        // Lock cursor within screen boundaries. 
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        // Reference to keybindings script.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();
    }
    
    void Update()
    {
        if (!m_pauseObject.activeInHierarchy)
        {
            // Get inputs from mouse axis.
            m_rotationX += _keys.GetMouseHorizontalAxis() * m_sensitivityX;
            m_rotationY -= _keys.GetMouseVerticalAxis() * m_sensitivityY;

            // Clamp Y rotation to 60 degrees in both directions.
            m_rotationY = Mathf.Clamp(m_rotationY, m_minYRotation, m_maxYRotation);

            // Apply smooth damping.
            m_rotationX_current = Mathf.SmoothDamp(m_rotationX_current, m_rotationX, ref m_rotationX_velocity, m_smoothDamp);
            m_rotationY_current = Mathf.SmoothDamp(m_rotationY_current, m_rotationY, ref m_rotationY_velocity, m_smoothDamp);

            // Apply camera rotation.
            m_camera_1.transform.rotation = Quaternion.Euler(m_rotationY_current, m_rotationX_current, 0.0f);
            m_camera_2.transform.rotation = Quaternion.Euler(m_rotationY_current, m_rotationX_current, 0.0f);
        }
    }

    private void FixedUpdate()
    {
        if (!m_pauseObject.activeInHierarchy)
        {
            //  Apply rotation of player around horizontal axis.
            m_rotation_1.rotation = Quaternion.Euler(0f, m_rotationX, 0f);
        }
    }

    private void OnDisabled()
    {
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
    }
    
    // Getters and setters.
    public float GetDamping()
    {
        return m_smoothDamp;
    }

    public void SetDamping(float value)
    {
        m_smoothDamp = value;
    }

    public float GetMinYRotation()
    {
        return m_minYRotation;
    }

    public void SetMinYRotation(float value)
    {
        m_minYRotation = value;
    }
}
