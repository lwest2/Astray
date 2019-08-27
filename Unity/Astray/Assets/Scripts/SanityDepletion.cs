using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Depletes sanity overtime.

public class SanityDepletion : MonoBehaviour
{
    // Player manager script.
    private Player_Manager _playerManager_script;

    // Begin depletion?
    [SerializeField]
    private bool beginDeplete = true;

    private void Start()
    {
        // Reference to player manager script.
        _playerManager_script = GameObject.Find("Manager_Player").GetComponent<Player_Manager>();

        // Invoke repeating method.
        InvokeRepeating("DepleteSanity", 0.0f, 2.5f);
    }
    

    void DepleteSanity()
    {
        // If allowed to deplete.
        if (beginDeplete)
        {
            // Take damage of 1 every 5 seconds.
            _playerManager_script.TakeDamage(1.0f);
        }
    }
}
