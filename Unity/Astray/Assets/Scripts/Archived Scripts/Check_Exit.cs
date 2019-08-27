using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
