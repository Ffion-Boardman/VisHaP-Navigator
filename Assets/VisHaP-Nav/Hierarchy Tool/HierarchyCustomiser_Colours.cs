using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.Hierarchy;

[InitializeOnLoad]
public class HierarchyCustomiser_Colours
{
    static HierarchyCustomiser_Colours()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    [MenuItem("GameObject/Add Hierarchy Style", false, -50)]
    private static void AddStyleComponent()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Only add the script if they don't already have one
            if (obj.GetComponent<HierarchyCustomiser_Settings>() == null)
            {
                obj.AddComponent<HierarchyCustomiser_Settings>();
            }
        }
    }
    
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        // Get the actual object being drawn
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;
        
        // Try to get settings component from the object
        HierarchyCustomiser_Settings settings = obj.GetComponent<HierarchyCustomiser_Settings>();
        
        // If the component exists, use it to draw the background and text
        if (settings != null)
        {
            DrawCustomRow(obj, selectionRect, settings);
        }
    }

    private static void DrawCustomRow(GameObject obj, Rect selectionRect, HierarchyCustomiser_Settings settings)
    {
        // Calculate the background area
        Rect fullRect = new Rect(selectionRect.x + 17, selectionRect.y, selectionRect.width, selectionRect.height);
        
        // Draw background
        EditorGUI.DrawRect(fullRect, settings.backgroundColor);
        
        // Configure text style
        GUIStyle customStyle = new GUIStyle(EditorStyles.label)
        {
            alignment = settings.alignment,
            fontStyle = GetFontStyle(settings.isBold, settings.isItalic),
            normal = { textColor = settings.textColor }
        };
        
        // Process the name of the object, if uppercase is enabled, make the text uppercase
        string nameToDisplay = settings.uppercase ? obj.name.ToUpper() : obj.name;
        
        // Draw the label
        EditorGUI.LabelField(fullRect, nameToDisplay, customStyle);
    }

    private static FontStyle GetFontStyle(bool bold, bool italic)
    {
        if (bold && italic) 
            return FontStyle.BoldAndItalic;
        else if (bold) 
            return FontStyle.Bold;
        else if (italic) 
            return FontStyle.Italic;
        else 
            return FontStyle.Normal;
    }
}