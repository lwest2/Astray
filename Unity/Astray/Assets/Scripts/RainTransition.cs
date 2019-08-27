using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Experimental.VFX;

// Transitions materials, terrain textures, fog, lighting and other settings when it rains.

public class RainTransition : MonoBehaviour
{
    // Rain object.
    [SerializeField]
    private GameObject m_rainObject;

    // Particle system associated with rain.
    private ParticleSystem m_rainSystem;

    // Game timer.
    private float m_gameTimer;
    // Seconds.
    private int m_seconds;
    // Test seconds.
    private float m_testSeconds = 0.0f;

    // Terrain Layers to smooth.
    [SerializeField]
    private TerrainLayer m_sticks;
    // Alpha 0.4 - 1.0
    [SerializeField]
    private TerrainLayer m_grassDried;
    // Alpha 0.2 - 1.0
    [SerializeField]
    private TerrainLayer m_grass;
    // Alpha 0.4 - 1.0
    [SerializeField]
    private TerrainLayer m_stones;
    // Alpha 0.3 - 1.0

    // Boolean to prevent repeats in Update function.
    private bool m_startOnce = true;

    // ObjectsWithinRain script.
    private ObjectsWithinRain _objectsWithinRain_script;
    // BerryBushSpawn script.
    private BerryState _berryBushSpawn_script;
    
    // Settings volume parameters.
    // Procedural Sky.
    ProceduralSky m_proceduralSky;

    // Density Volume
    DensityVolume m_densityVolume;

    private float m_elapsedTime = 0.0f;
    private float m_time = 10.0f;

    private void Start()
    {
        // rainObject particle system.
        m_rainSystem = m_rainObject.GetComponent<ParticleSystem>();
        // stop rain at the beginning of the game.
        m_rainSystem.Stop();

        // Reset smoothness values of terrain layers at the beginning.
        m_sticks.maskMapRemapMin = new Vector4(0, 0, 0, 0);
        m_grassDried.maskMapRemapMin = new Vector4(0, 0, 0, 0);
        m_grass.maskMapRemapMin = new Vector4(0, 0, 0, 0);
        m_stones.maskMapRemapMin = new Vector4(0, 0, 0, 0);
        
        // Game timer set to current time.
        m_gameTimer = Time.time;

        // Invoke random number generation every 20 seconds.
        InvokeRepeating("GenerateRandomNumber", 0.0f, 20.0f);

        // Reference objectsWithinRain script.
        _objectsWithinRain_script = GameObject.Find("Collisions").GetComponent<ObjectsWithinRain>();
        // Reference berryBushSpawn script.
        _berryBushSpawn_script = GameObject.Find("Berry & Shroom spawnpoints").GetComponent<BerryState>();

        // Grab procedural sky parameter from volume.
        GameObject.Find("Scene Settings").GetComponent<Volume>().profile.TryGet(out m_proceduralSky);

        // Grab density volume.
        m_densityVolume = GameObject.Find("Density Volume").GetComponent<DensityVolume>();
    }

    private void Update()
    {
        // If time is more than gamerTimer + 1.
        if (Time.time > m_gameTimer + 1)
        {
            // Set gameTimer to time.
            m_gameTimer = Time.time;

            // Add seconds. (Is typically a counter.)
            m_seconds++;
        }

        // If seconds is more or equal to testSeconds and testSeconds is more than 0.
        if (m_seconds >= m_testSeconds && m_testSeconds > 0.0f)
        {
            // Seconds is 0.
            m_seconds = 0;

            // If rain is stopped.
            if (m_rainSystem.isStopped)
            {
                // Start rain system.
                Debug.Log("Starting rain.");
                m_rainSystem.Play();

                // Start once.
                if (m_startOnce)
                {
                    m_startOnce = false;

                    // Grow Berries
                    GrowBerries();

                    // Stop current coroutine if playing already.
                    StopCoroutine("TransitionTextures");

                    // Start a new coroutine with these variables for minimum and maximum.
                    StartCoroutine(TransitionTextures(0.0f, 0.5f, 0.0f, 0.3f, 0.0f, 0.4f, 13.8f, 8.5f, 0.85f, 2.5f, 1.61f, -1.0f));
                }
            }
            else
            {
                // Else stop rain system.
                Debug.Log("Stopping rain.");
                m_rainSystem.Stop();

                // Start once.
                if (m_startOnce)
                {
                    m_startOnce = false;

                    // Stop current coroutine if playing already.
                    StopCoroutine("TransitionTextures");

                    // Start a new coroutine with these variables for minimum and maximum.
                    StartCoroutine(TransitionTextures(0.5f, 0.0f, 0.3f, 0.0f, 0.4f, 0.0f, 8.5f, 13.8f, 2.5f, 0.85f, -1.0f, 1.61f));
                }
            }
        }
    }

    //  Generate random number within range.
    void GenerateRandomNumber()
    {
        m_testSeconds = Random.Range(25.0f, 40.0f);
        //m_testSeconds = Random.Range(10.0f, 30.0f);
    }

    // Coroutine for transitioning textures between minimum and maximum values.
    private IEnumerator TransitionTextures(float stickMin, float stickMax, float grassMin, float grassMax, float stoneMin, 
        float stoneMax, float fogMin, float fogMax, float atmosMin, float atmosMax, float exposureMin, float exposureMax)
    {
        // Make elapsedTime equal to 0.
        Debug.Log("In coroutine: TransitionTextures");
        m_elapsedTime = 0.0f;

        // If elapsedTime is less than maximum time.
        while (m_elapsedTime < m_time)
        {
            // Lerp terrain layers depending on min and max values.
            m_sticks.maskMapRemapMin = new Vector4(0, 0, 0, Mathf.Lerp(stickMin, stickMax, m_elapsedTime / m_time));
            m_grassDried.maskMapRemapMin = new Vector4(0, 0, 0, Mathf.Lerp(grassMin, grassMax, m_elapsedTime / m_time));
            m_stones.maskMapRemapMin = new Vector4(0, 0, 0, Mathf.Lerp(grassMin, grassMax, m_elapsedTime / m_time));

            // Lerp procedural sky depending on min and max values.
            m_proceduralSky.atmosphereThickness.value = Mathf.Lerp(atmosMin, atmosMax, m_elapsedTime / m_time);
            m_proceduralSky.exposure.value = Mathf.Lerp(exposureMin, exposureMax, m_elapsedTime / m_time);

            // Lerp fog distance depending on min and max values.
            m_densityVolume.parameters.meanFreePath = Mathf.Lerp(fogMin, fogMax, m_elapsedTime / m_time);

            // If objectsWithinRain list has a length more than and exists.
            if (_objectsWithinRain_script.m_objectsWithinRadius.Count > 0 && _objectsWithinRain_script.m_objectsWithinRadius != null)
            {
                // If rain is stopped.
                if (m_rainSystem.isStopped)
                {
                    // Change materials on the list (smoothness values, decrease)
                    ChangeMaterials(0.0f, 0.1f, 0.0f);

                    Debug.Log("Removing");
                    _objectsWithinRain_script.m_objectsWithinRadius.Clear();
                        
                    
                }
                else
                {
                    // Change materials on list (smoothness values, increase)
                    ChangeMaterials(0.1f, 0.2f, 0.32f);
                }
            }
            // increase elapsedTime based on time.
            m_elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Can start coroutine again.
        m_startOnce = true;
        yield return null;
    }

    
    private void ChangeMaterials(float min, float max, float coatValMax)
    {
        // For each  gameObject in list.
        foreach (GameObject i in _objectsWithinRain_script.m_objectsWithinRadius)
        {

            // get range of materials in i.
            int range = i.GetComponent<Renderer>().materials.Length;

            Renderer objectRenderer = i.GetComponent<Renderer>();
        
            // for each of these materials.
            for (int j = 0; j < range; j++)
            {
                
                // If the material has specific property
                if (objectRenderer.materials[j].HasProperty("_SmoothnessRemapMin"))
                {
                    // Set the min and max values.
                    objectRenderer.materials[j].SetFloat("_SmoothnessRemapMin", min);
                    objectRenderer.materials[j].SetFloat("_SmoothnessRemapMax", max);
                    
                    // Room for improvement:
                    // Lerp.
                    // Also coat map.
                    // Revert back to original smoothness values.
                }
                else if (objectRenderer.materials[j].HasProperty("_Smoothness"))
                {
                    // Else if it has _Smoothness property instead, set this value.
                    objectRenderer.materials[j].SetFloat("_Smoothness", max);
                }
                

                /*
                if (objectRenderer.materials[j].HasProperty("_CoatMask"))
                {
                    objectRenderer.materials[j].SetFloat("_CoatMask", coatValMax);
                }
                */
            }
            
        }
    }  
    
    private void GrowBerries()
    {
        // If list exists and is more than 0.
        _berryBushSpawn_script.ActivateBerries(true);
    }
}

