using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

// Turn the fire on if it is not raining.

public class ToggleFire : MonoBehaviour
{

    // Fire parameters.
    // Boolean fireOn?
    private bool m_fireOn = true;

    // VFX Graph effect.
    private VisualEffect m_fireEffect;

    // Static variables for exposed parameters.
    private static readonly string m_smoke = "Smoke";
    private static readonly string m_spark = "Sparks";

    // Light attached to fire.
    private Light m_fireLight;

    // Particle system associated with rain.
    private ParticleSystem m_rainSystem;

    // Rain object.
    private GameObject m_rainObject;

    private AudioSource m_audioSource;

    private void Start()
    {
        // Grab fire VFX graph effect & light.
        m_fireEffect = GameObject.Find("Fire").GetComponent<VisualEffect>();
        m_fireLight = GameObject.Find("Fire Light").GetComponent<Light>();

        // Rain Object
        m_rainObject = GameObject.Find("Rain");
        // rainObject particle system.
        m_rainSystem = m_rainObject.GetComponent<ParticleSystem>();

        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!m_rainSystem.isStopped)
        {
            if (other.gameObject.name == "RainEffect")
            {
                // if fire is not already out, extinguish fire
                if (m_fireOn)
                {
                    m_fireOn = false;
                    // change smoke capacity from 10 to 0
                    // change spark capacity from 10 to 0
                    // disable light
                    m_fireEffect.SetFloat(m_smoke, 0);
                    m_fireEffect.SetFloat(m_spark, 0);
                    m_fireLight.enabled = false;
                    m_audioSource.Stop();
                    Debug.Log("Turning off fire.");
                }
            }
        }
    }

    public void TurnFireOn()
    {
        m_fireOn = true;
        // change smoke capacity from o to 10
        // change spark capacity from 0 to 10
        // disable light
        m_fireEffect.SetFloat(m_smoke, 10);
        m_fireEffect.SetFloat(m_spark, 10);
        m_fireLight.enabled = true;
        m_audioSource.Play();
        Debug.Log("Turning on fire.");
    }

    public bool GetFireOn()
    {
        return m_fireOn;
    }

    public void SetFireOn(bool value)
    {
        m_fireOn = value;
    }
}
