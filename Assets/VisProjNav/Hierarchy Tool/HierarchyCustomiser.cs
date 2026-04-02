using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Hierarchy;

[InitializeOnLoad]
public class HierarchyCustomiser : MonoBehaviour
{
    static HierarchyCustomiser()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }
    
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        // Get the actual object being drawn
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;
        
        // Header logic (if the nape starts with '---' or is in ALL CAPS)
        if (obj.name.StartsWith("---"))
        {
            // Draw a background colour that fills in the width of the header
            Rect headerRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 100, selectionRect.height);
            Rect fullRect = new Rect(32, selectionRect.y, selectionRect.width + (selectionRect.x - 32), selectionRect.height);
            EditorGUI.DrawRect(fullRect, new Color(0.2f, 0.2f, 0.2f, 0.7f));
            
            // Draw the text in the center
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            EditorGUI.LabelField(headerRect, obj.name.Replace("-", "").ToUpper(), headerStyle);
        }
    }
}
