using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Change various mechanics and post processing effects of the game depending on the players sanity.

public class StateBehaviour : MonoBehaviour
{
    // Booleans for phases.
    public bool m_phaseOne = false;
    public bool m_phaseTwo = false;

    // Variables for phase one.
    public float m_p1Anxiety = 0.0f;
    public float m_p1LOConcentration = 0.0f;
    public float m_p1Depression = 0.0f;

    // Variables for phase two.
    public float m_p2Delusions = 0.0f;
    public float m_p2Hallucinations = 0.0f;

    // Boolean isLerping?
    private bool m_isLerping = false;

    // Decide random lerp position.
    private float m_randomLerp = 0.0f;
    // Hold original depth of field.
    private float m_originalDepthLerp = 0.0f;

    // Post processing behaviour gameObject.
    public GameObject m_postProcessing;

    // Post processing behaviour effects set to null.
    AmbientOcclusion m_ambientOcclusionLayer = null;
    AutoExposure m_autoExposureLayer = null;
    Bloom m_bloomLayer = null;
    ChromaticAberration m_chromaticAberrationLayer = null;
    ColorGrading m_colorGradingLayer = null;
    DepthOfField m_depthOfFieldLayer = null;
    Grain m_grainLayer = null;
    LensDistortion m_lensDistortionLayer = null;
    MotionBlur m_motionBlurLayer = null;
    ScreenSpaceReflections m_screenSpaceReflectionsLayer = null;
    Vignette m_vignetteLayer = null;

    // Look rotation script.
    private PlayerController_LookRot _lookRot_script;
    // Player manager script.
    private Player_Manager _playerManager_script;

    // Used to get original values of post processing behaviours, instead of hardcoded values.
    private float m_depthOfField_val;
    private float m_autoExposure_val;
    private float m_motionBlue_val;
    private float m_colorGrading_temp_val;
    private float m_colorGrading_saturation_val;
    private float m_vignette_val;

    // Multipliers that effect variables.
    private float m_anxietyMulti = 1.0f;
    private float m_depressionMulti = 1.2f;
    private float m_locMulti = 0.2f;
    private float m_halucinationMulti = 0.1f;
    private float m_delusionMulti = 0.1f;

    private ParticleSystem m_meshEffect;
    private ParticleSystem.EmissionModule m_em;

    private AudioSource m_voices;
    private bool m_triggerOnce = true;

    private void Start()
    {
        // Get settings of post processing behaviour for editing.
        PostProcessVolume volume = GameObject.Find("Post Processing").GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out m_ambientOcclusionLayer);
        volume.profile.TryGetSettings(out m_autoExposureLayer);
        volume.profile.TryGetSettings(out m_bloomLayer);
        volume.profile.TryGetSettings(out m_chromaticAberrationLayer);
        volume.profile.TryGetSettings(out m_colorGradingLayer);
        volume.profile.TryGetSettings(out m_depthOfFieldLayer);
        volume.profile.TryGetSettings(out m_grainLayer);
        volume.profile.TryGetSettings(out m_lensDistortionLayer);
        volume.profile.TryGetSettings(out m_motionBlurLayer);
        volume.profile.TryGetSettings(out m_screenSpaceReflectionsLayer);
        volume.profile.TryGetSettings(out m_vignetteLayer);

        // Hold values of original post processing effect values.
        m_depthOfField_val = m_depthOfFieldLayer.focalLength.value;
        m_autoExposure_val = m_autoExposureLayer.minLuminance.value;
        m_motionBlue_val = m_motionBlurLayer.shutterAngle.value;
        m_colorGrading_temp_val = m_colorGradingLayer.temperature.value;
        m_colorGrading_saturation_val = m_colorGradingLayer.saturation.value;
        m_vignette_val = m_vignetteLayer.smoothness.value;

        // Scripts dependencies.
        _lookRot_script = GameObject.Find("Manager_Player").GetComponent<PlayerController_LookRot>();
        _playerManager_script = GameObject.Find("Manager_Player").GetComponent<Player_Manager>();

        m_meshEffect = GameObject.Find("Distortion").GetComponent<ParticleSystem>();
        m_em = m_meshEffect.emission;
        m_em.rateOverTime = 0.0f;

        m_voices = GameObject.Find("Voices").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (m_phaseOne)
        {
            // When sanity decreases certain post processing effects occur.
            GeneralInsanity();

            // Apply Anxiety Features
            AnxietyEffects();

            // Apply Lack of Concentration Features
            LOConcentrationEffects();

            // Apply Depression Features
            DepressionEffects();

        }
        else
        {
            // Set back to defaults.
            Defaults();
        }

        if (m_phaseTwo)
        {
            // Hallucinations
            HallucinationsEffects();

            // Delusions
            DelusionEffects();
        }
        else
        {
            SecondDefaults(); 
        }
    }

    void HallucinationsEffects()
    {
        // Mesh effects turn on after threshold is met.
        /*
        if (m_p2Hallucinations > 0.04)
        {
            if (!m_meshEffect.activeInHierarchy)
            {
                m_meshEffect.SetActive(true);
            }
        }
        else
        {
            if (m_meshEffect.activeInHierarchy)
            {
                m_meshEffect.SetActive(false);
            }
        }
        */

        float distortVal = Mathf.Lerp(0.0f, 10.0f, m_p2Hallucinations * 3);
        m_em.rateOverTime = distortVal;
    }

    void DelusionEffects()
    {
        // Audio for delusions increase.
        if (m_p2Delusions > 0.04f && m_triggerOnce)
        {
            m_triggerOnce = false;
            m_voices.Play();
        }
    }

    void AnxietyEffects()
    {
        // Look rotation speed decreases / damping becomes increased.
        float dampValue = Mathf.Lerp(0.1f, 1.0f, m_p1Anxiety / 2);
        _lookRot_script.SetDamping(dampValue);

        // Breathing increases (Sound).

        // Players head rotation locks further down
        float minLockRot = Mathf.Lerp(-60.0f, 20.0f, m_p1Anxiety);
        _lookRot_script.SetMinYRotation(minLockRot);
    }

    void LOConcentrationEffects()
    {
        // Depth of Field
        // If layer is available, and concentration is not equal to 0.
        if (m_depthOfFieldLayer && m_p1LOConcentration != 0.0f)
        {
            // If not active, set active.
            if (!m_depthOfFieldLayer.active)
            {
                m_depthOfFieldLayer.active = true;
            }

            // Lerp random chance based on LOConcentration variable.
            float randomChanceMax = Mathf.Lerp(20.0f, 1.0f, m_p1LOConcentration);

            // Get a random number between 0 and lerped variable.
            float randomChance = Random.Range(0.0f, randomChanceMax);

            // If random chance is less or equal to 1.0f and is not already lerping.
            if (randomChance <= 1.0f && !m_isLerping)
            {
                // Now lerping.
                m_isLerping = true;

                // Grab original lerp value.
                m_originalDepthLerp = m_depthOfFieldLayer.focalLength.value;

                // Grab a value to lerp between, also multiplied (effected) by LOConcentration variable.
                m_randomLerp = Random.Range(25.0f, 115.0f * m_p1LOConcentration);
            }

            // If should be lerping.
            if (m_isLerping)
            {
                // Lerp depth of field focal length value depending on the random lerp value generated, previously.
                m_depthOfFieldLayer.focalLength.value = Mathf.Lerp(m_depthOfFieldLayer.focalLength.value, m_randomLerp, Time.deltaTime * 0.85f);

                // If the depth of field is within 2.0f, give or take.
                if ((m_randomLerp + 2) > m_depthOfFieldLayer.focalLength.value && m_depthOfFieldLayer.focalLength.value > (m_randomLerp - 2))
                {
                    // Turn lerping off.
                    m_isLerping = false;
                }
            }
        }
        else
        {
            // Else make depth of field value 0.
            m_depthOfFieldLayer.focalLength.value = 0.0f;
        }

        // Minimum UV decreased.          
        if (m_autoExposureLayer)
        {
            // If not active.
            if (!m_autoExposureLayer.active)
            {
                // Set layer active.
                m_autoExposureLayer.active = true;
            }
            // Lerp exposure based on LOConcentration variable.
            m_autoExposureLayer.minLuminance.value = Mathf.Lerp(-m_autoExposure_val, -3.33f, m_p1LOConcentration);
        }


        // Random sounds become more prominent, such as breathing, birds, trees. (Sound)


        // Slight increase in motion blur.
        if (m_motionBlurLayer)
        {
            // If not active.
            if (!m_motionBlurLayer.active)
            {
                // Set layer active.
                m_motionBlurLayer.active = true;
            }
            // Lerp shutterAngle based on LOConcentration variable.
            m_motionBlurLayer.shutterAngle.value = Mathf.Lerp(m_motionBlue_val, 360.0f, m_p1LOConcentration);
        }
    }

    void DepressionEffects()
    {
        // Color grading becomes colder.
        if (m_colorGradingLayer)
        {
            // If not active.
            if (!m_colorGradingLayer.active)
            {
                // Set layer active.
                m_colorGradingLayer.active = true;
            }
            // Lerp temperature and saturation based on Depression variable.
            m_colorGradingLayer.temperature.value = Mathf.Lerp(m_colorGrading_temp_val, -25.0f, m_p1Depression * 2);
            m_colorGradingLayer.saturation.value = Mathf.Lerp(m_colorGrading_saturation_val, -62.0f, m_p1Depression);
        }


        // Crying / Suicidal thoughts (Sound)


        // Vignetta increases to result in a more narrow, darker view. 
        if (m_vignetteLayer)
        {
            // If not active.
            if (!m_vignetteLayer.active)
            {
                // Set layer active.
                m_vignetteLayer.active = true;
            }
            // Lerp vignette based on Depression variable.
            m_vignetteLayer.smoothness.value = Mathf.Lerp(m_vignette_val, 0.44f, m_p1Depression);
        }

    }

    void GeneralInsanity()
    {
        if (m_vignetteLayer)
        {
            // If not active.
            if (!m_vignetteLayer.active)
            {
                // Set layer active.
                m_vignetteLayer.active = true;
            }

            // Grab player health and inverse it.
            float inversedSanity = _playerManager_script.GetHealthMax() - _playerManager_script.GetHealth();

            // Create a red vignette layer and lerp it. 
            m_vignetteLayer.color.value = Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time, 5.0f));

            // Alter multipliers to have different effects of anxiety, LOC, and depression.
            m_p1Anxiety = (inversedSanity / 100.0f) * m_anxietyMulti;
            m_p1LOConcentration = (inversedSanity / 100.0f) * m_locMulti;
            m_p1Depression = (inversedSanity / 100.0f) * m_depressionMulti;
            // Likewise for halucinations and delusions.
            m_p2Delusions = (inversedSanity / 100.0f) * m_delusionMulti;
            m_p2Hallucinations = (inversedSanity / 100.0f) * m_halucinationMulti;
        }
    }

    

    void Defaults()
    {
        // Look rot damping. 
        _lookRot_script.SetDamping(0.1f);

        // Look rot clamp Y.
        _lookRot_script.SetMinYRotation(-60.0f);

        // Reset Post processing effects.
        m_depthOfFieldLayer.focalLength.value = m_depthOfField_val;
        m_autoExposureLayer.minLuminance.value = -m_autoExposure_val;
        m_motionBlurLayer.shutterAngle.value = m_motionBlue_val;
        m_colorGradingLayer.temperature.value = m_colorGrading_temp_val;
        m_colorGradingLayer.saturation.value = m_colorGrading_saturation_val;
        m_vignetteLayer.smoothness.value = m_vignette_val;
        m_vignetteLayer.color.value = Color.black;
    }

    void SecondDefaults()
    {
        // Set everything back to defaults.
    }
}
