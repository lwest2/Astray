using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Displays a message on the UI.
public class DisplayMessage : MonoBehaviour
{
    private float m_elapsedTime;
    private float m_newPos;

    private Coroutine m_coroutine = null;

    [SerializeField]
    private GameObject m_textObject;
    private TextMeshProUGUI m_textMesh;

    private Vector4 m_startPos;

    private void Start()
    {
        m_textMesh = m_textObject.GetComponent<TextMeshProUGUI>();

        // Get margins of textMesh.
        m_startPos = m_textMesh.margin;

        // Start message
        ResetAndStartCoroutine("I should explore... I need food... or warmth... ", 0.5f, 7.0f);
    }

    public void ResetAndStartCoroutine(string message, float time, float wait_time)
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
            Debug.Log("STOPPING COROUTINE.");
            // Reset parameters.
        }

        m_coroutine = StartCoroutine(NotifyPlayer(message, time, wait_time));

    }

    // Coroutine for processing messages in a modular manner.
    public IEnumerator NotifyPlayer(string message, float time, float wait_time)
    {
        // Check if UI already displays.
        if (m_textObject.activeInHierarchy)
        {
            m_textObject.SetActive(false);
        }

        // Set Text
        m_textMesh.text = message;

        // UI turns off after a few seconds.
        m_elapsedTime = 0.0f;

        // While loop for coroutine.
        while (m_elapsedTime < time)
        {
            // Lerp new margin size overtime from start position to 0.0f;
            m_newPos = Mathf.Lerp(m_startPos.z, 0.0f, (m_elapsedTime / time));

            // Apply margin position.
            m_textMesh.margin = new Vector4(m_textMesh.margin.x, m_textMesh.margin.y, m_newPos, m_textMesh.margin.w);

            // If no UI displays dialogue.
            m_textObject.SetActive(true);

            // Increased elapsed time.
            m_elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for defined seconds.
        yield return new WaitForSeconds(wait_time);

        // Toggle off message, and return textMesh margin to default. 
        m_textObject.SetActive(false);
        m_textMesh.margin = m_startPos;

        yield return null;
    }
}
