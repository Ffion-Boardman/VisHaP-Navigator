using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.VersionControl;

[InitializeOnLoad]
public class FolderColourManager
{
    private const string PREFS_PREFIX = "FolderColour_";

    static FolderColourManager()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }
    
    // Adding the context menu (the 2000 is the 'priority' so I can choose where it appears in the context menu)
    [MenuItem("Assets/Folder Colour/None", false, 2000)]
    private static void SetColourNone() => SaveColour("");

    [MenuItem("Assets/Folder Colour/Red", false, 2001)]
    private static void SetColourRed() => SaveColour("#FF3B30");
    
    [MenuItem("Assets/Folder Colour/Orange", false, 2002)]
    private static void SetColorOrange() => SaveColour("#FF9500");

    [MenuItem("Assets/Folder Colour/Yellow", false, 2003)]
    private static void SetColorYellow() => SaveColour("#FFCC00");

    [MenuItem("Assets/Folder Colour/Green", false, 2004)]
    private static void SetColorGreen() => SaveColour("#4CD964");

    [MenuItem("Assets/Folder Colour/Blue", false, 2005)]
    private static void SetColorBlue() => SaveColour("#007AFF");

    [MenuItem("Assets/Folder Colour/Purple", false, 2006)]
    private static void SetColorPurple() => SaveColour("#5856D6");
    
    [MenuItem("Assets/Folder Colour/Pink", false, 2007)]
    private static void SetColorPink() => SaveColour("#FF69B4");
    
    // Showing the menu only for folders, no other assets
    [MenuItem("Assets/Folder Colour/", true)]
    private static bool ValidateFolder() => Selection.activeObject != null && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(Selection.activeObject));

    private static void SaveColour(string hex)
    {
        string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Selection.activeObject));
        
        // If there's 'none' selected, then just make it blank
        if (string.IsNullOrEmpty(hex))
        {
            EditorPrefs.DeleteKey(PREFS_PREFIX + guid);
        }
        else
        {
            EditorPrefs.SetString(PREFS_PREFIX + guid, hex);
        }
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        string hex = EditorPrefs.GetString(PREFS_PREFIX + guid, "");
        
        if (string.IsNullOrEmpty(hex)) return;

        if (ColorUtility.TryParseHtmlString(hex, out Color colour))
        {
            // Making it semi-transparent
            colour.a = 0.4f;
            
            // Adjust rect so it covers the icon specifically
            bool isSmall = selectionRect.height < 20;
            
            // Seeing if they're little folders or big folders
            Rect iconRect = isSmall
                ? new Rect(selectionRect.x, selectionRect.y, 16, 16)
                : new Rect(selectionRect.x + 3, selectionRect.y, selectionRect.width - 6, selectionRect.width - 6);
            
            EditorGUI.DrawRect(iconRect, colour);
        }
    }
}
