using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkyRotation : MonoBehaviour
{

    private GameObject m_moon;
    private GameObject m_sun;
    private GameObject m_middle;

    // Script for displaying messages. 
    private DisplayMessage _displayMessage_script;

    // Slider
    private Slider m_slider;
    private float m_elapsedTime;

    private bool m_triggerOnce = true;

    // Parameters to send to coroutine.
    private float m_time = 0.5f;
    private float m_wait_time = 1.0f;
    private string m_message = null;

    // Start is called before the first frame update
    void Start()
    {
        m_middle = GameObject.Find("SkyRotation");
        m_sun = GameObject.Find("Sun");
        m_moon = GameObject.Find("Moon");
        m_slider = GameObject.Find("Slider").GetComponent<Slider>();
        m_slider.value = 0;

        // Script for displaying messages.
        _displayMessage_script = GameObject.Find("DisplayText").GetComponent<DisplayMessage>();

        StartCoroutine("EndGameCountdown");
    }

    private void Update()
    {
        // Orbit sun around object for day cycle to be complete.
        m_sun.transform.RotateAround(Vector3.zero, Vector3.right, 0.5f * Time.deltaTime);

        //m_moon.transform.RotateAround(Vector3.zero, Vector3.right, 1f * Time.deltaTime);
       
    }

    private IEnumerator EndGameCountdown()
    {

        // While loop for coroutine.
        while (m_slider.value < 340.0f)
        {
            if (m_slider.value == 300.0f)
            {
                _displayMessage_script.ResetAndStartCoroutine("10 seconds until dusk...", m_time, m_wait_time);
            }

            if (m_slider.value == 305.0f)
            {
                _displayMessage_script.ResetAndStartCoroutine("5 seconds until dusk...", m_time, m_wait_time);
            }

            yield return new WaitForSeconds(1.0f);
            m_slider.value++;
                

            yield return null;
        }

        // Return to game complete scene.
        SceneManager.LoadScene(3, LoadSceneMode.Single);

        yield return null;
    }
}
