using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
[InitializeOnLoad]
public class FolderCustomiser
{
    // For keeping folder names and their desired colours
    private static readonly Dictionary<string, Color> FolderSettings = new Dictionary<string, Color>
    {
        { "Scripts", new Color(0.3f, 0.7f, 1f, 0.4f) },
        { "Prefabs", new Color(0.3f, 1f, 0.4f, 0.4f) },
        { "Scenes", new Color(1f, 0.4f, 0.4f, 0.4f) },
        { "UI", new Color(1f, 0.9f, 0.3f, 0.4f) }
    };

    static FolderCustomiser()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        // Get the file-paths
        string path = AssetDatabase.GUIDToAssetPath(guid);
        
        // Only target the folders, nothing else
        if (!AssetDatabase.IsValidFolder(path)) return;

        // Get the folder names
        string folderName = System.IO.Path.GetFileName(path);
        if (FolderSettings.ContainsKey(folderName))
        {
            // Define the area for the small icon (which is usually 16x16)
            Rect iconRect = new Rect(selectionRect.x, selectionRect.y, 16, 16);
            
            // If the view is 'Large Icons', the rect logic is different
            if (selectionRect.height > 20)
            {
                iconRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.width);
            }
            
            // Draw a semi-transparent colour over the folder
            EditorGUI.DrawRect(iconRect, FolderSettings[folderName]);
        }
    }
}
*/