using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// UI listeners to switch between scenes. Also includes pause menu.

public class ButtonClick : MonoBehaviour
{
    // Buttons from UI.
    public Button m_startGame, m_settings, m_controls, m_quitGame, m_returnToGame, m_unpause;

    // Canvas Group.
    private CanvasGroup m_canvasGroup;

    // Reference to pauseEsc script.
    private pauseEsc _pauseEsc_script;

    private void OnEnable()
    {
        // Unlock cursor and make visible.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Start()
    {
        if (GameObject.Find("Manager_Player"))
        {
            _pauseEsc_script = GameObject.Find("Manager_Player").GetComponent<pauseEsc>();
        }
        
        // Add listeners to buttons.
        if (m_startGame != null)
        {
            m_startGame.onClick.AddListener(StartGame);
        }

        if (m_settings != null)
        {
            m_settings.onClick.AddListener(Settings);
        }

        if (m_controls != null)
        {
            m_controls.onClick.AddListener(Controls);
        }

        if (m_quitGame != null)
        {
            m_quitGame.onClick.AddListener(QuitGame);
        }

        if (m_returnToGame != null)
        {
            m_returnToGame.onClick.AddListener(ReturnToGame);
        }

        if (m_unpause != null)
        {
            m_unpause.onClick.AddListener(UnpauseGame);
        }

        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    void StartGame()
    {
        //StartCoroutine("Starting");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    void Settings()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    void Controls()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void ReturnToGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    void UnpauseGame()
    {
        if (_pauseEsc_script != null)
        {
            _pauseEsc_script.Pause();
        }
    }

    /*
    private IEnumerator Starting()
    {
        Debug.Log("Fading");
        // UI turns off after a few seconds.
        float m_elapsedTime = 0.0f;
        float time = 3.0f;

        // While loop for coroutine.
        while (m_elapsedTime < time)
        {

            m_canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, (m_elapsedTime / time));
            
            if (m_canvasGroup.alpha < 0.03)
            {
                m_canvasGroup.alpha = 0;
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }

            // Increased elapsed time.
            m_elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    */
}
