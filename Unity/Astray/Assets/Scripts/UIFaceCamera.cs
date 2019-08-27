using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Have the world space UI objects face the camera then fade out when in close proximity.

public class UIFaceCamera : MonoBehaviour
{
    // Reference: https://answers.unity.com/questions/181000/gui-text-always-facing-camera.html

    [SerializeField]
    private Camera m_camera;

    private Camera m_UIcamera;

    private Transform m_transform;

    private CanvasGroup m_canvasGroup;

    private float maxAlphaSolid = 15.0f;
    private void Start()
    {
        m_transform = GetComponent<Transform>();
        m_camera = GameObject.Find("Main Camera_2").GetComponent<Camera>();
        m_UIcamera = GameObject.Find("UI Camera").GetComponent<Camera>();

        if (!GetComponent<Canvas>().worldCamera)
        {
            GetComponent<Canvas>().worldCamera = m_UIcamera;
        }

        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        // Face towards camera.
        Vector3 position = m_camera.transform.position - transform.position;

        position.x = 0;
        position.z = 0;

        m_transform.LookAt(m_camera.transform.position - position);
        m_transform.Rotate(0, 180, 0);

        // If within close proximity fade or turn off.
        Vector3 heading = m_camera.transform.position - m_transform.position;
        float dist = heading.magnitude;
 
        if (dist < maxAlphaSolid)
        {
            dist = maxAlphaSolid;
        }

        float calc = 1.0f - (maxAlphaSolid / dist);

        if (calc < 0.0f)
        {
            calc = 0.0f;
        }

        m_canvasGroup.alpha = calc;

        /*
    if (dist < 10.0f && isActiveAndEnabled)
    {
        gameObject.SetActive(false);
    }
    else
    {
        gameObject.SetActive(true);
    }
    */
    }
}
