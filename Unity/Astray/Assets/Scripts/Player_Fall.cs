using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Calculate fall damage of the player if the player has significant airtime.
public class Player_Fall : MonoBehaviour
{
    // Player manage script.s
    private Player_Manager _playerManager;
    
    // Character controller.
    private CharacterController m_characterController;

    private float m_airTime = 0f;               // Time within air.
    private float m_minDamageTime = 1.5f;       // minimum fall time to take damage
    private float m_minDeathTime = 3.0f;        // minimum fall time to take damage
    private float m_damagePerSeconds = 10.0f;   // 10 damage per second in air

    // Damage to send to player manager.
    private float m_damage; 

    void Start()
    {
        // Reference to player manager script.
        _playerManager = GameObject.Find("Manager_Player").GetComponent<Player_Manager>();
        
        // Reference to character controller.
        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // If character controller is not grounded.
        if (!m_characterController.isGrounded)
        {
            // Increase airTime by deltaTime.
            m_airTime += Time.deltaTime;
        }
        else if (m_characterController.isGrounded)
        {
            // Else if grounded, and airtime is more than minDamageTime.
            if (m_airTime > m_minDamageTime)
            {
                // If airTime is more than minDeathTime.
                if (m_airTime > m_minDeathTime)
                {
                    // Damage player based on current health. (kill player)
                    m_damage = _playerManager.GetHealth();

                    _playerManager.TakeDamage(m_damage);
                }
                else
                {
                    // Damage calculated based on the damagePerSeconds multiplied by the time in air.
                    m_damage = m_damagePerSeconds * m_airTime;

                    // Take damage based on this formula.
                    _playerManager.TakeDamage(m_damage);
                }
            }
            // Reset airtime.
            m_airTime = 0;
        }
    }

}
