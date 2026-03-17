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
        
    }
}
