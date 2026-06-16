using UnityEngine;
using UnityEditor;

// Tells Unity to use this layout whenever the Settings component is clicked on
[CustomEditor(typeof(HierarchyCustomiser_Settings))]
[CanEditMultipleObjects] // Means a user can select multiple objects and style them at once!
public class HierarchyCustomiser_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Hierarchy Appearence", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
        // Draw the standard properties
        EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("textColor"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("uppercase"));
        
        GUILayout.Space(5);
        
        // - - - FONT STYLE - - -
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Style");

        SerializedProperty isBold = serializedObject.FindProperty("isBold");
        SerializedProperty isItalic = serializedObject.FindProperty("isItalic");
        
        // Style the text inside the buttons to be bold and italic respectively
        GUIStyle boldStyle = new GUIStyle(EditorStyles.miniButtonLeft) { fontStyle = FontStyle.Bold };
        GUIStyle italicStyle = new GUIStyle(EditorStyles.miniButtonRight) { fontStyle = FontStyle.Italic };
        
        // The toggle draws a button that remains 'pressed down' when the value is true
        isBold.boolValue = GUILayout.Toggle(isBold.boolValue, new GUIContent("B", "Bold"), boldStyle, GUILayout.Width(40));
        isItalic.boolValue = GUILayout.Toggle(isItalic.boolValue, new GUIContent("I", "Italic"), italicStyle, GUILayout.Width(40));
        
        EditorGUILayout.EndHorizontal();
        
        // - - - FONT ALIGNMENT - - -
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Alignment");
        
        SerializedProperty alignment = serializedObject.FindProperty("alignment");
        
        // Fun math trick I learned: split the 0-8 text anchor numbers into X and Y coordinates
        int currentAnchor = alignment.intValue;
        int vert = currentAnchor / 3;  // 0 = Top,  1 = Middle, 2 = Bottom
        int horiz = currentAnchor % 3; // 0 = Left, 1 = Center, 2 = Right
        
        // Guide:
        //    L   C   R
        // T  0   1   2
        // M  3   4   5
        // B  6   7   8
        
        // Fetching Unity's built-in horizontal-alignment icons for the tooltips
        GUIContent iconLeft = new GUIContent(EditorGUIUtility.IconContent("GUISystem/align_horizontally_left").image, "Left Align");
        GUIContent iconCenter = new GUIContent(EditorGUIUtility.IconContent("GUISystem/align_horizontally_center").image, "Center Align");
        GUIContent iconRight = new GUIContent(EditorGUIUtility.IconContent("GUISystem/align_horizontally_right").image, "Right Align");
        
        // Horizontal Toggle
        EditorGUI.BeginChangeCheck();
        bool left = GUILayout.Toggle(horiz == 0, iconLeft, EditorStyles.miniButtonLeft, GUILayout.Width(25));
        bool center = GUILayout.Toggle(horiz == 1, iconCenter, EditorStyles.miniButtonMid, GUILayout.Width(25));
        bool right = GUILayout.Toggle(horiz == 2, iconRight, EditorStyles.miniButtonRight, GUILayout.Width(25));

        if (EditorGUI.EndChangeCheck())
        {
            if (left && horiz!= 0) horiz = 0;
            else if (center && horiz!= 1) horiz = 1;
            else if (right && horiz!= 2) horiz = 2;
        }
        
        GUILayout.Space(5);
        
        // Fetching Unity's built-in vertical-alignment icons for the tooltips
        GUIContent iconTop = new GUIContent(EditorGUIUtility.IconContent("GUISystem/align_vertically_top").image, "Top Align");
        GUIContent iconMiddle = new GUIContent(EditorGUIUtility.IconContent("GUISystem/align_vertically_center").image, "Middle Align");
        GUIContent iconBottom = new GUIContent(EditorGUIUtility.IconContent("GUISystem/align_vertically_bottom").image, "Bottom Align");
        
        // Vertical Toggle
        EditorGUI.BeginChangeCheck();
        bool top = GUILayout.Toggle(vert == 0, iconTop, EditorStyles.miniButtonLeft, GUILayout.Width(25));
        bool middle = GUILayout.Toggle(vert == 1, iconMiddle, EditorStyles.miniButtonMid, GUILayout.Width(25));
        bool bottom = GUILayout.Toggle(vert == 2, iconBottom, EditorStyles.miniButtonRight, GUILayout.Width(25));
        
        if (EditorGUI.EndChangeCheck())
        {
            if (top && vert != 0) vert = 0;
            else if (middle && vert != 1) vert = 1;
            else if (bottom && vert != 2) vert = 2;
        }
        
        // Rebuild that int we divided up earlier and save it back to the property
        alignment.intValue = (vert * 3) + horiz;
        
        EditorGUILayout.EndHorizontal();
        
        // - - - PRESETS - - -
        
        GUILayout.Space(15);
        EditorGUILayout.LabelField("Presets", EditorStyles.boldLabel);
        
        GUILayout.BeginHorizontal(); // Puts buttons side-by-side
        
        if (GUILayout.Button("Default"))
        {
            ApplyPreset(
                new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f), 
                new Color(0.7921569f, 0.7921569f, 0.7921569f, 1f), 
                TextAnchor.MiddleLeft, 
                false, 
                false, 
                false);
        }
        
        if (GUILayout.Button("Header"))
        {
            ApplyPreset(
                new Color(0.2f, 0.2f, 0.2f, 1f), 
                Color.white, 
                TextAnchor.MiddleLeft, 
                true, 
                false, 
                false);
        }

        if (GUILayout.Button("Divider"))
        {
            ApplyPreset(
                new Color(0.7921569f, 0.7921569f, 0.7921569f, 1f), 
                new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f), 
                TextAnchor.MiddleCenter, 
                true, 
                true, 
                false);
        }
        
        GUILayout.EndHorizontal();

        if (serializedObject.ApplyModifiedProperties())
        {
            EditorApplication.delayCall += EditorApplication.RepaintHierarchyWindow;
        }
    }

    private void ApplyPreset(Color background, Color text, TextAnchor alignment, bool isBold, bool isUpper, bool isItalic)
    {
        // Registers the change in engine so you can hit Ctrl+Z ro undo it
        Undo.RecordObjects(targets, "Apply Hierarchy Preset");
        
        // Loop through all selected objects and apply the preset
        foreach (HierarchyCustomiser_Settings settings in targets)
        {
            settings.backgroundColor = background;
            settings.textColor = text;
            settings.alignment = alignment;
            settings.isBold = isBold;
            settings.uppercase = isUpper;
            settings.isItalic = isItalic;
            
            // Save the changes to the scene
            EditorUtility.SetDirty(settings);
        }
        
        EditorApplication.delayCall += EditorApplication.RepaintHierarchyWindow;
    }
}
