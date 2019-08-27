using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All keybindings of the game.

public class KeyBindings : MonoBehaviour
{
    // Player inputs
    private float m_horizontalAxis;

    private float m_verticalAxis;

    private bool m_sneakBool;

    private bool m_runBool;

    private bool m_jumpBool;

    private bool m_crouchBool;

    private float m_mouse_horizontalAxis;

    private float m_mouse_verticalAxis;

    private bool m_viewBool;

    private bool m_interactBool;

    private bool m_pauseBool;

    // Update is called once per frame
    void Update()
    {
        // Player inputs for movement
        m_horizontalAxis = Input.GetAxis("Horizontal");
        m_verticalAxis = Input.GetAxis("Vertical");

        // Player inputs for sneak and bool
        m_runBool = Input.GetKey(KeyCode.LeftShift);
        m_sneakBool = Input.GetKey(KeyCode.LeftControl);

        // Player inputs for jump
        m_jumpBool = Input.GetKey(KeyCode.Space);

        // Player inputs for crouch
        m_crouchBool = Input.GetKeyDown(KeyCode.LeftAlt);

        // Player inputs for mouse rotation 
        m_mouse_horizontalAxis = Input.GetAxis("Mouse X");
        m_mouse_verticalAxis = Input.GetAxis("Mouse Y");

        // Player input for toggle view
        m_viewBool = Input.GetKeyDown(KeyCode.F);

        // Player input for interactions.
        m_interactBool = Input.GetKeyDown(KeyCode.E);

        // Player input for pausing.
        m_pauseBool = Input.GetKeyDown(KeyCode.Escape);
    }

    // Setters and Getters.
    public float GetHorizontalAxis()
    {
        return m_horizontalAxis;
    }

    public float GetVerticalAxis()
    {
        return m_verticalAxis;
    }

    public bool GetRunBool()
    {
        return m_runBool;
    }

    public bool GetSneakBool()
    {
        return m_sneakBool;
    }

    public bool GetJumpBool()
    {
        return m_jumpBool;
    }

    public bool GetCrouchBool()
    {
        return m_crouchBool;
    }

    public float GetMouseHorizontalAxis()
    {
        return m_mouse_horizontalAxis;
    }

    public float GetMouseVerticalAxis()
    {
        return m_mouse_verticalAxis;
    }

    public bool GetViewBool()
    {
        return m_viewBool;
    }

    public bool GetInteractBool()
    {
        return m_interactBool;
    }

    public bool GetPauseBool()
    {
        return m_pauseBool;
    }
}
