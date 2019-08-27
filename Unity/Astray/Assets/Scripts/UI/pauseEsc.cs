using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Pauses the game when the ESC key is pressed.

public class pauseEsc : MonoBehaviour
{
    [SerializeField]
    private GameObject m_pauseMenu;

    private KeyBindings _keys;

    private bool m_pauseBool;

    [SerializeField]
    private Button m_pauseButton;

    private void OnEnable()
    {
        Time.timeScale = 1;
        if (m_pauseMenu.activeInHierarchy)
        {
            m_pauseMenu.SetActive(false);
        }
    }
    
    void Start()
    {
        // Reference to keybindings.
        _keys = GameObject.Find("Manager_Input").GetComponent<KeyBindings>();

        if (m_pauseButton != null)
        {
            m_pauseButton.onClick.AddListener(BackToMain);
        }
    }

    void Update()
    {
        GetInputs();

        if (m_pauseBool)
        {
            Pause();
        }
    }

    void GetInputs()
    {
        m_pauseBool = _keys.GetPauseBool();
    }

    public void Pause()
    {
        if (Time.timeScale == 1)
        {
            // Pause time, open pause menu and unlock cursor.
            Time.timeScale = 0;
            m_pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        } else if (Time.timeScale == 0)
        {
            // Make timescale 1, close pause menu and lock cursor.
            Time.timeScale = 1;
            m_pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void BackToMain()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
