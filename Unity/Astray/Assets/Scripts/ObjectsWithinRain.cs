using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds objects to a list when they are within the rain.

public class ObjectsWithinRain : MonoBehaviour
{
    // List to hold objects within radius.
    public List<GameObject> m_objectsWithinRadius = new List<GameObject>();

    // Rain object.
    private GameObject m_rainObject;
    // Particle system associated with rain.
    private ParticleSystem m_rainSystem;

    private void Start()
    {
        // Rain Object
        m_rainObject = GameObject.Find("Rain");
        // rainObject particle system.
        m_rainSystem = m_rainObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Call method with radius as parameter.
        if (!m_rainSystem.isStopped)
        {
            GetObjectsInRadius(15.0f);
        }
    }


    private void GetObjectsInRadius(float radius)
    {
        // Temporary array of hit colliders.
        Collider[] hit;

        // Get colliders within the given radius of the rain game object.
        hit = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z), radius);

        // For each of these colliders.
        foreach (Collider i in hit)
        {
            // If object is not within array,
            // and tagged as environment,
            // and contains component Renderer
            if (!m_objectsWithinRadius.Contains(i.gameObject) &&
                i.gameObject.CompareTag("Environment") &&
                i.gameObject.GetComponent<Renderer>())
            {
                Debug.Log("Object added: " + i.gameObject);
                // Add object to list.
                m_objectsWithinRadius.Add(i.gameObject);
            }            
        }
    }   
}
