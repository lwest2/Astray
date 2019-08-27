using UnityEngine;
using UnityEditor;

// StateBehaviour.
[CustomEditor(typeof(StateBehaviour))]
public class State_Window : Editor
{

    public override void OnInspectorGUI()
    {
        // Window code
        StateBehaviour myStateBehaviour_script = (StateBehaviour)target;

        // Phase 1
        GUILayout.Space(15.0f);
        GUILayout.Label("Phase 1 - Prodrome", EditorStyles.boldLabel);

        myStateBehaviour_script.m_phaseOne = EditorGUILayout.BeginToggleGroup("Toggle Phase 1 settings", myStateBehaviour_script.m_phaseOne);

        myStateBehaviour_script.m_p1Anxiety = EditorGUILayout.Slider("Anxiety", myStateBehaviour_script.m_p1Anxiety, 0.0f, 1.0f);

        myStateBehaviour_script.m_p1LOConcentration = EditorGUILayout.Slider("Lack of Concentration", myStateBehaviour_script.m_p1LOConcentration, 0.0f, 1.0f);

        myStateBehaviour_script.m_p1Depression = EditorGUILayout.Slider("Depression", myStateBehaviour_script.m_p1Depression, 0.0f, 1.0f);

        EditorGUILayout.EndToggleGroup();

        // Phase 2
        GUILayout.Space(15.0f);
        GUILayout.Label("Phase 2 - Psychotic Phase", EditorStyles.boldLabel);

        myStateBehaviour_script.m_phaseTwo = EditorGUILayout.BeginToggleGroup("Toggle Phase 2 settings", myStateBehaviour_script.m_phaseTwo);

        myStateBehaviour_script.m_p2Delusions = EditorGUILayout.Slider("Delusions", myStateBehaviour_script.m_p2Delusions, 0.0f, 1.0f);

        myStateBehaviour_script.m_p2Hallucinations = EditorGUILayout.Slider("Hallucinations", myStateBehaviour_script.m_p2Hallucinations, 0.0f, 1.0f);

        EditorGUILayout.EndToggleGroup();

        GUILayout.FlexibleSpace();

        // If reset button pressed.
        if (GUILayout.Button("Reset"))
        {
            // Reset values.
            myStateBehaviour_script.m_p1Anxiety = 0.0f;
            myStateBehaviour_script.m_p1LOConcentration = 0.0f;
            myStateBehaviour_script.m_p1Depression = 0.0f;
            myStateBehaviour_script.m_p2Delusions = 0.0f;
            myStateBehaviour_script.m_p2Hallucinations = 0.0f;
            myStateBehaviour_script.m_phaseOne = false;
            myStateBehaviour_script.m_phaseTwo = false;
        }
    }


}
