using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer_Tribe : MonoBehaviour
{
    [SerializeField]
    private GameObject m_player;

    private Transform m_transform;

    // Start is called before the first frame update
    void OnEnable()
    {
        m_transform = GetComponent<Transform>();
        Instantiate(m_player, m_transform.position, m_transform.rotation);
    }
}
