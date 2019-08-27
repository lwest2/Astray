using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarGraph : MonoBehaviour
{
    Mesh m_mesh;

    private float m_sanity_y = 1.0f;

    private float m_hunger_x = -1.0f;
    private float m_hunger_y = 1.0f;

    private float m_thirst_x = 1.0f;
    private float m_thirst_y = 1.0f;

    [SerializeField]
    private Material m_material;

    private void Start()
    {
        m_mesh = new Mesh();
        
        m_mesh.vertices = new Vector3[] {
            new Vector3(m_hunger_x, m_hunger_y, 0),
            new Vector3(0, m_sanity_y, 0),
            new Vector3(m_thirst_x, m_thirst_y, 0)
        };

        m_mesh.triangles = new int[] { 0, 1, 2 };

        m_mesh.uv = new Vector2[] {
            new Vector2(m_hunger_x, m_hunger_y),
            new Vector2(0, m_sanity_y),
            new Vector2(m_thirst_x, m_thirst_y)
        };

        m_mesh.RecalculateNormals();

        GetComponent<Renderer>().material = m_material;
        GetComponent<MeshFilter>().mesh = m_mesh;

        InvokeRepeating("TickUpdateRadarGraph", 0f, 1f);
    }

    void TickUpdateRadarGraph()
    {
        if (m_sanity_y < 50f)
        {
            m_sanity_y += 1f;
        }

        if (m_hunger_x > -50f)
        {
            m_hunger_x -= 1f;
            m_hunger_y -= 1f;
        }

        if (m_thirst_x < 50f)
        {
            m_thirst_x += 1f;
            m_thirst_y -= 1f;
        }

        m_mesh.vertices = new Vector3[] { new Vector3(m_hunger_x, m_hunger_y, 0), new Vector3(0, m_sanity_y, 0), new Vector3(m_thirst_x, m_thirst_y, 0) };
        m_mesh.triangles = new int[] { 0, 1, 2 };
        m_mesh.uv = new Vector2[] {
            new Vector2(m_hunger_x, m_hunger_y),
            new Vector2(0, m_sanity_y),
            new Vector2(m_thirst_x, m_thirst_y)
        };
        m_mesh.RecalculateNormals();


        GetComponent<Renderer>().material = m_material;
        GetComponent<MeshFilter>().mesh = m_mesh;
    }
}
