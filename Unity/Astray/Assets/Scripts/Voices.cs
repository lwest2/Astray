using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voices : MonoBehaviour
{
    private GameObject m_camera;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GameObject.Find("Main Camera_2");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(m_camera.transform.position, Vector3.up, 60 * Time.deltaTime);
    }
}
