using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAndUpdateMeshes : MonoBehaviour
{
    [SerializeField]
    private GameObject m_meshPrefab;

    [SerializeField]
    private GameObject[] m_environmentObjects;

    // Start is called before the first frame update
    void Start()
    {
        m_environmentObjects = GameObject.FindGameObjectsWithTag("Environment");
        
        foreach (GameObject i in m_environmentObjects)
        {
            if (i.GetComponent<MeshRenderer>())
            {
                GameObject temp = Instantiate(m_meshPrefab) as GameObject;
                temp.SetActive(true);
                temp.transform.parent = i.transform;
                PSMeshRendererUpdater psUpdater = temp.GetComponent<PSMeshRendererUpdater>();
                psUpdater.UpdateMeshEffect(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
