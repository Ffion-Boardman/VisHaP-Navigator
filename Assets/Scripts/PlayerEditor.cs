using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private SerializedProperty healthProperty;
    private SerializedProperty speedProperty;

    private void OnEnable()
    {
        // Cache references to serialized properties. Here "health" and "speed" match the identifiers
        healthProperty = serializedObject.FindProperty("health");
        speedProperty = serializedObject.FindProperty("speed");
    }
    
    public override void OnInspectorGUI()
    {
        // Refresh serialised state
        serializedObject.Update();
        
        // Show sliders for health & speed
        EditorGUILayout.Slider(healthProperty, 0f, 100f);
        EditorGUILayout.Slider(speedProperty, 0f, 100f);
        
        // Show a label for debugging the actual value
        EditorGUILayout.LabelField($"hp = {healthProperty.floatValue}, speed = {speedProperty.floatValue}");
        
        // Apply modified changes
        serializedObject.ApplyModifiedProperties();
    }
}
