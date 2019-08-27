using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Berry lifespan. Changes berry type over time from ripe, to old, then finally destroys them.

public class BerryTimer : MonoBehaviour
{
    private float m_randomTimer;

    [SerializeField]
    private GameObject m_berries_new;

    [SerializeField]
    private GameObject m_berries_old;

    // BerryState script.
    private BerryState _berryBushSpawn_kill_script;

    public void GrowBerries()
    {
        m_randomTimer = Random.Range(20.0f, 30.0f);

        Invoke("GrowOld", m_randomTimer);
    }

    private void Start()
    {
        // Reference berrystate script.
        _berryBushSpawn_kill_script = GameObject.Find("Berry & Shroom spawnpoints").GetComponent<BerryState>();
    }

    void GrowOld()
    {
        Debug.Log("Turning berries old.");
        if (_berryBushSpawn_kill_script.CheckBerryActive(transform))
        {
            _berryBushSpawn_kill_script.ChangeBerry(m_berries_new, m_berries_old);

            Invoke("KillBerries", 20.0f);
        }
    }

    void KillBerries()
    {
        Debug.Log("Killing berries.");
        _berryBushSpawn_kill_script.ActivateSingleBerry(transform, false);
    }
}
