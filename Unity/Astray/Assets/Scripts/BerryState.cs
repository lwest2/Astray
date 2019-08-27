using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Activates berries and completes numerous checks to make sure that the berries are switched accordingly throughout their lifespan.
public class BerryState : MonoBehaviour
{
    // BerryBushSpawn script.
    private BerryBushSpawn _berryBushSpawn_script;

    private void Start()
    {
        // Reference berryBushSpawn script.
        _berryBushSpawn_script = GameObject.Find("Berry & Shroom spawnpoints").GetComponent<BerryBushSpawn>();
    }

    // Activates all berries within the scene.
    public void ActivateBerries(bool state)
    {
        Debug.Log("Activating berries.");

        // If list exists and is more than 0.
        if (_berryBushSpawn_script.m_bushList != null && _berryBushSpawn_script.m_bushList.Count > 0)
        {
            // For each gameobject inside list.
            foreach (GameObject obj in _berryBushSpawn_script.m_bushList)
            {
                // Get the second child.
                GameObject tempChild = obj.transform.GetChild(1).gameObject;

                Debug.Log("Berry child(1): " + tempChild);
                // if child is not active in hierarchy enable it and spawn berries.
                if (!tempChild.activeInHierarchy)
                {
                    tempChild.SetActive(state);
                    Debug.Log("Turning on/off Berries.");
                }
            }
        }
    }

    // Activates berries of a specific berry bush.
    public void ActivateSingleBerry(Transform berryBush, bool state)
    {
        Transform[] children = berryBush.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.gameObject.name == "Berries" || child.gameObject.name == "Berries (old)")
            {
                child.gameObject.SetActive(state);
            }
        }
    }
    
    /*
    public void ChangeMaterial(Material mat, Transform berries)
    {
        Transform[] children = berries.GetComponentsInChildren<Transform>();

        // For each gameobject inside list.
        foreach (Transform child in children)
        {
            Debug.Log("i: " + child.gameObject.name);

            if (child.gameObject.name == "Berry")
            {
                Debug.Log("Changing Colour!");
                child.gameObject.GetComponent<Renderer>().material = mat;
            }            
        }
    }
    */

    // Changes the berry state from one berry type to the next.
    public void ChangeBerry(GameObject currentBerry, GameObject nextBerry)
    {
        currentBerry.SetActive(false);
        nextBerry.SetActive(true);
    }

    /*
    public bool CheckMaterial(Transform berryBush)
    {
        bool isNew = false;

        Transform[] children = berryBush.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.gameObject.name == "Berry")
            {
                if (child.gameObject.GetComponent<Renderer>().material.name == "Berries (Instance)")
                {
                    Debug.Log("Material is true!");
                    isNew = true;
                    break;
                }
            }
        }
        
        Debug.Log("Material: " + isNew);
        return isNew;
    }
    */

    // Checks what berries are currently on the berry bush.
    public bool CheckBerry(Transform berryBush)
    {
        bool isNew = false;

        Transform[] children = berryBush.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.gameObject.name == "Berries" && child.gameObject.activeInHierarchy)
            {
                isNew = true;
            }
        }

        return isNew;
    }

    // Checks if berries are active on a specific berry bush.
    public bool CheckBerryActive(Transform berryBush)
    {
        bool berryActive = false;

        Transform[] children = berryBush.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.gameObject.name == "Berries" || child.gameObject.name == "Berries (old)")
            {
                if (child.gameObject.activeInHierarchy)
                {
                    berryActive = true;
                    break;
                }
            }
        }
            
        return berryActive;
    }
}
