using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Hierarchy;

[InitializeOnLoad]
public class HierarchyCustomiser_Colours : MonoBehaviour
{
    static HierarchyCustomiser_Colours()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }
    
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        // Get the actual object being drawn
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;
        
        // Header Logic
        // (if the name starts with '---')
        if (obj.name.StartsWith("---"))
        {
            // Draw a background colour that fills in the width of the header
            Rect headerRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 100, selectionRect.height);
            Rect boxRect = new Rect(selectionRect.x + 17, selectionRect.y, selectionRect.width, selectionRect.height);
            Rect fullRect = new Rect(32, selectionRect.y, selectionRect.width + (selectionRect.x - 32), selectionRect.height);
            EditorGUI.DrawRect(boxRect, new Color(0.2f, 0.2f, 0.2f, 1f));
            
            // Draw the text in the center
            GUIStyle headerStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = Color.white }
            };
            EditorGUI.LabelField(headerRect, obj.name.Replace("-", "  ")/*.ToUpper()*/, headerStyle);
        }
        
        // Title Logic
        // (if the name starts with '===')
        if (obj.name.StartsWith("==="))
        {
            // Draw a background colour that fills in the width of the header
            Rect headerRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
            Rect boxRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
            Rect fullRect = new Rect(32, selectionRect.y, selectionRect.width + (selectionRect.x - 32), selectionRect.height);
            EditorGUI.DrawRect(boxRect, new Color(0.372549f, 0.6196079f, 0.627451f, 1f));
            
            // Draw the text in the center
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            EditorGUI.LabelField(headerRect, obj.name.Replace("=", "").ToUpper(), headerStyle);
        }
    }
}
