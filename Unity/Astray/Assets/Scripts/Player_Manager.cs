using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    // Health (Sanity)
    private float m_health;
    private float m_healthMax = 100.0f;

    // Slider
    private Slider m_slider;

    // Is dead?
    private bool m_isDead = false;

    private float m_regenTimer = 10.0f;         // Timer until regen.
    private float m_regenInvokeTimer = 0.0f;    // Invoke repetition starts at 0 seconds.
    private float m_regenInvokeRepeat = 2.0f;   // Repeat method every 5 seconds.
    private bool m_canRegen = true;             // Can regen?

    // Mesh effect
    [SerializeField]
    private GameObject m_effectInstance;

    [SerializeField]
    private GameObject m_meshObject;

    private void OnEnable()
    {
        Cursor.visible = false;
    }
    void Start()
    {
        // Health is equal to max health at start of the game.
        m_health = m_healthMax;
        m_slider = GameObject.Find("SanityBar").GetComponent<Slider>();
        m_slider.value = m_healthMax;

        //StartCoroutine(LateChange(false));

    }

    /*
    private IEnumerator LateChange(bool state)
    {
        yield return new WaitForSeconds(2f);

        PSMeshRendererUpdater psUpdater = m_effectInstance.GetComponent<PSMeshRendererUpdater>();
        psUpdater.IsActive = state;

        yield return null;
    }
    */

    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        // If not dead, take damage. 
        if (!m_isDead)
        {
            if (!((m_health + -damage) > m_healthMax))
            {
                // Take away damage from health.
                SetHealth(-damage);
               

                Debug.Log("Health: " + m_health);
            } else
            {
                m_health = m_healthMax;
            }

            // if health is equal or less to 0.
            if (m_health <= 0)
            {
                m_health = 0;
                // Dead is equal to true.
                m_isDead = true;
                Debug.Log("Player is dead.");
            }
        } else
        {
            SceneManager.LoadScene(4, LoadSceneMode.Single);
        }
    }

    public void StartRegen()
    {
        if (!IsInvoking("Regenerate"))
        {
            //StopCoroutine("LateChange");
            //StartCoroutine(LateChange(true));
            InvokeRepeating("Regenerate", 0.0f, 2.0f);
        }
    }

    public void StopRegen()
    {
        Debug.Log("Test 5");
        if (IsInvoking("Regenerate"))
        {
            Debug.Log("Test 5.1");
            CancelInvoke("Regenerate");
            //StopCoroutine("LateChange");
            //StartCoroutine(LateChange(false));
        }
    }

    /*
    private void Regen()
    {
        // Regen health over time, if not hit after time elapsed

        StartCoroutine(RegenHealth());
    }

    // Method parameters used by Regenerate method.
    private IEnumerator RegenHealth()
    {
        m_canRegen = false; // Currently cannot regen

        yield return new WaitForSeconds(m_regenTimer);

        m_canRegen = true; // After time elapsed after damage taken, can now regen
    }
    */

    // Invoked method.
    private void Regenerate()
    {
        // If can regen and health is less than maximum health
        if (m_canRegen && m_health < m_healthMax)
        {
            // Add 1 health.
            SetHealth(2);
            Debug.Log("Increased Health: " + m_health);
        }
    }

    // Getters and setters.
    public float GetHealth()
    {
        return m_health;
    }
    
    public void SetHealth(float value)
    {
        m_health += value;
        // Also increase slider value depending on damage taken.
        m_slider.value = m_health;
        
    }

    public float GetHealthMax()
    {
        return m_healthMax;
    }

    public void SetHealthMax(float value)
    {
        m_healthMax = value;
    }
}
