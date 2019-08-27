using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns berry bushes in the scene.

public class BerryBushSpawn : MonoBehaviour
{
    // Berry bush object.
    [SerializeField]
    private GameObject m_berryBush;

    // Spawn points array for berry bush.
    private Transform[] m_spawnPoints;

    // Spawn points array for active spawns containing a berry bush.
    private List<Transform> m_activeSpawnPointsList = new List<Transform>();

    // Random chance variable.
    private int m_randomChance = 0;

    // List of bushes that were instantiated.
    public List<GameObject> m_bushList = new List<GameObject>();

    private void Start()
    {
        // Get components in children.
        m_spawnPoints = GameObject.Find("Berry & Shroom spawnpoints").GetComponentsInChildren<Transform>();

        // For each of these components, add them to active spawn list.
        foreach (Transform i in m_spawnPoints)
        {
            m_activeSpawnPointsList.Add(i);
        }

        // Do while active bushes is less than 3.
        do
        {
            // for each item in the list.
            Debug.Log("Spawning berries");
            for(int i = 0; i < m_activeSpawnPointsList.Count; i++)
            {
                // Calculate random chance to instantiate a berry bush.
                m_randomChance = Random.Range(0, 2);

                if (m_randomChance == 1)
                {
                    GameObject temp = Instantiate(m_berryBush, m_activeSpawnPointsList[i].position, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
                    m_activeSpawnPointsList.RemoveAt(i);
                    m_bushList.Add(temp);
                }
            }
        } while (m_activeSpawnPointsList.Count < 3);
    }
}
