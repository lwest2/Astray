using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rain follows player around the map.

public class RainFollow : MonoBehaviour
{
    // Target to follow.
    [SerializeField]
    private GameObject m_target;

    // Transform of rain.
    private Transform m_transform;

    // Vector for newPosition.
    private Vector3 m_newPos;

    private void Start()
    {
        // Get transform component.
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        // newPos equals target position.
        m_newPos = new Vector3(m_target.transform.position.x, m_transform.position.y, m_target.transform.position.z);

        // Move towards this position over time.
        m_transform.position = Vector3.MoveTowards(m_transform.position, m_newPos, Time.deltaTime * 1.5f);
    }
}
