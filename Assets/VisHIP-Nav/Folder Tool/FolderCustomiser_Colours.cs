using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.VersionControl;

[InitializeOnLoad]
public class FolderCustomiser_Colours
{
    private const string PREFS_PREFIX = "FolderColour_";

    static FolderCustomiser_Colours()
    {
        EditorApplication.projectWindowItemInstanceOnGUI += OnProjectWindowItemGUI;
    }
    
    #region Default Colour List
    
    // Adding the context menu (the 2000 is the 'priority', so I can choose where it appears in the context menu)
    // Note: if the gap between priority numbers is larger than 11, a line-separator will be added
    
    [MenuItem("Assets/Folder Colour/None", false, 2000)]
    private static void SetColourNone() => SaveColour("");

    [MenuItem("Assets/Folder Colour/Red", false, 2011)]
    private static void SetColourRed() => SaveColour("#FF3B30");
    
    [MenuItem("Assets/Folder Colour/Orange", false, 2012)]
    private static void SetColorOrange() => SaveColour("#FF9500");

    [MenuItem("Assets/Folder Colour/Yellow", false, 2013)]
    private static void SetColorYellow() => SaveColour("#FFCC00");

    [MenuItem("Assets/Folder Colour/Green", false, 2014)]
    private static void SetColorGreen() => SaveColour("#4CD964");

    [MenuItem("Assets/Folder Colour/Blue", false, 2015)]
    private static void SetColorBlue() => SaveColour("#007AFF");

    [MenuItem("Assets/Folder Colour/Purple", false, 2016)]
    private static void SetColorPurple() => SaveColour("#5856D6");
    
    [MenuItem("Assets/Folder Colour/Pink", false, 2017)]
    private static void SetColorPink() => SaveColour("#FF69B4");
    
    [MenuItem("Assets/Folder Colour/More Colours...", false, 2030)]
    private static void OpenCustomColorPicker()
    {
        CustomFolderColorWindow.OpenWindow();
    }
    
    #endregion
    
    // Showing the menu only for folders, no other assets
    [MenuItem("Assets/Folder Colour/None", true)]
    [MenuItem("Assets/Folder Colour/Red", true)]
    [MenuItem("Assets/Folder Colour/Orange", true)]
    [MenuItem("Assets/Folder Colour/Yellow", true)]
    [MenuItem("Assets/Folder Colour/Green", true)]
    [MenuItem("Assets/Folder Colour/Blue", true)]
    [MenuItem("Assets/Folder Colour/Purple", true)]
    [MenuItem("Assets/Folder Colour/Pink", true)]
    [MenuItem("Assets/Folder Colour/More Colours...", true)]
    private static bool ValidateFolder()
    {
        // Hide the menu if nothing is selected
        if (Selection.assetGUIDs.Length == 0) return false;
        
        // Only show the menu if there is at least one folder selected
        foreach (string guid in Selection.assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            
            if (AssetDatabase.IsValidFolder(path)) return true;
        }
        
        return false;
    }

    private static void SaveColour(string hex)
    {
        // Loop through all selected objects (so users can colour multiple folders at once)
        foreach (string guid in Selection.assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            
            // If there's anything that ISN'T a folder selected alongside the folder, skip over it
            if (!AssetDatabase.IsValidFolder(path)) continue;
            
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
        
        // Making the project window repaint immediately
        EditorApplication.RepaintProjectWindow();
    }

    private static void OnProjectWindowItemGUI(int instanceID, Rect selectionRect)
    {
        // Because we're starting with an instance, we need to convert the numeric instance back to the project path string
        string path = AssetDatabase.GetAssetPath(instanceID);
        if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path)) return;
        
        // Getting if the GUID of the folder has a saved colour choice already or not
        string guid = AssetDatabase.AssetPathToGUID(path);
        
        string hex = EditorPrefs.GetString(PREFS_PREFIX + guid, "");
        if (string.IsNullOrEmpty(hex)) return;

        if (ColorUtility.TryParseHtmlString(hex, out Color colour))
        {
            // Calculate the exact dimensions of the icon
            bool isSmall = selectionRect.height < 20;
            Rect iconRect;

            if (isSmall)
            {
                iconRect = new Rect(selectionRect.x, selectionRect.y, 16, 16);
            }
            else
            {
                iconRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.width);
            }
            
            // Check the 'state' of the folder (e.g., if it's empty or open)
            bool isEmpty = IsEmptyFolder(path);
            bool isExpanded = false;
            
            // The open folder only appears as a small icon specifically, so we have to avoid it for the large icons
            if (!isEmpty && isSmall)
            {
                // Check Unity's internal array of expanded tree-view folders
                int[] expandedIDs = UnityEditorInternal.InternalEditorUtility.expandedProjectWindowItems;
                if (System.Array.IndexOf(expandedIDs, instanceID) >= 0)
                {
                    isExpanded = true;
                }
            }
            
            // Fetch the right texture based on the current UI state
            Texture2D folderIcon = null;

            // Use the native sprites built into Unity
            if (isEmpty)
            {
                folderIcon = EditorGUIUtility.IconContent("FolderEmpty Icon").image as Texture2D;
            }
            else if (isExpanded)
            {
                folderIcon = EditorGUIUtility.IconContent("FolderOpened Icon").image as Texture2D;
            }
            else
            {
                // The default icon for Unity folders is the full, unopened icon
                folderIcon = AssetDatabase.GetCachedIcon(path) as Texture2D;
            }
            
            if (folderIcon == null) return;
            
            // Caching the original editor UI colour
            Color originalColour = GUI.color;
            
            // Apply the tint over it
            colour.a = 1.0f;
            GUI.color = colour;
            
            // Draw folder texture over it
            GUI.DrawTexture(iconRect, folderIcon);
            
            // Turn the colour back to normal so there's no bleeding that tints everything
            GUI.color = originalColour;
        }
    }

    private static bool IsEmptyFolder(string path)
    {
        // Make sure the folder actually exists (just to avoid insane crashes and stuff)
        if (!System.IO.Directory.Exists(path)) return false;
        
        var items = System.IO.Directory.GetFileSystemEntries(path);

        foreach (var item in items)
        {
            // Unity generates hidden .meta files for folders, so if there's something in there that ISN'T a .meta, then the folder is empty
            if (!item.EndsWith(".meta")) return false;
        }

        return true;
    }

    public class CustomFolderColorWindow : EditorWindow
    {
        // Setting it to white by default
        private Color chosenColour = Color.white;
        
        // Recent-Colours Variables
        private const string RECENTS_PREF_KEY = "FolderColour_Recents";
        private List<string> recentColours = new List<string>();
        private const int MAX_RECENTS = 14;
        
        // Default Colours Grid
        private readonly string[][] defaultColours = new string[][]
        {
            //               Grey       Red        Orange     Yellow     Green      Blue       Purple 
            new string[] { "#000000", "#FF1111", "#FF8800", "#FFDD00", "#44DD44", "#1177FF", "#6633CC" }, // Deep
            new string[] { "#444444", "#FF4444", "#FFAA33", "#FFEE33", "#77EE77", "#4499FF", "#8855DD" }, // Vibrant
            new string[] { "#888888", "#FF7777", "#FFCC66", "#FFFF66", "#AAFFAA", "#77BBFF", "#AA88EE" }, // Medium
            new string[] { "#CCCCCC", "#FFAAAA", "#FFDD99", "#FFFF99", "#CCFFCC", "#AADDFF", "#CCAAFF" }, // Pastel
            new string[] { "#FFFFFF", "#FFDDDD", "#FFEECC", "#FFFFCC", "#EEFFEE", "#DDEEFF", "#EEDDFF" }  // Pale
        };

        public static void OpenWindow()
        {
            // Create a window on screen with the name 'Custom Folder Colour'
            CustomFolderColorWindow window = GetWindow<CustomFolderColorWindow>("Custom Folder Colour");
            
            // Setting rigid dimensions for the layout of the window
            window.minSize = new Vector2(250, 360);
            window.maxSize = new Vector2(250, 360);
            
            // Opens as a 'floating' utility window, so it doesn't get docked into the layout tabs like the Inspector
            window.ShowUtility();
        }

        private void OnEnable()
        {
            // Whenever this menu is opened, it should show the past 'recent colours'
            LoadRecents();
        }

        private void OnDisable()
        {
            // Save the currently picked custom colour to the recents
            string hex = "#" + ColorUtility.ToHtmlStringRGBA(chosenColour);
            AddRecentColour(hex);
        }

        private void OnGUI()
        {
            // - - - DEFAULT COLOURS - - -
            
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Default Colours", EditorStyles.boldLabel);
            
            // Default Colours Grid
            GUILayout.BeginVertical();
            for (int row = 0; row < defaultColours.Length; row++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                for (int col = 0; col < defaultColours[row].Length; col++)
                {
                    DrawColour(defaultColours[row][col]);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            DrawSeparator();
            
            // - - - RECENT COLOURS - - -
            
            EditorGUILayout.LabelField("Recent Colours", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            
            // Make sure there ARE recent colours
            if (recentColours.Count == 0)
            {
                GUILayout.Label("No recent colours found.", EditorStyles.miniLabel);
            }
            else
            {
                // Draw the recent swatches, wrapping to a new line every 7 colours
                for (int rowStart = 0; rowStart < recentColours.Count; rowStart += 7)
                {
                    // Start a new row
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    
                    // Find how many colours go in this row
                    int itemsInThisRow = Mathf.Min(7, recentColours.Count - rowStart);
                    
                    // Draw just the items for this row, none of the others
                    for (int i = 0; i < itemsInThisRow; i++)
                    {
                        DrawColour(recentColours[rowStart + i]);
                    }
                    
                    // Close the row
                    GUILayout.EndHorizontal();
                }
            }
            
            GUILayout.EndHorizontal();

            DrawSeparator();
            
            // - - - CUSTOM COLOURS - - -
            
            EditorGUILayout.LabelField("Custom Colour", EditorStyles.boldLabel);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUI.BeginChangeCheck();
            chosenColour = EditorGUILayout.ColorField(chosenColour);

            if (EditorGUI.EndChangeCheck())
            {
                string hex = "#" + ColorUtility.ToHtmlStringRGB(chosenColour);
                FolderCustomiser_Colours.SaveColour(hex);
            }
            
            GUILayout.EndHorizontal();
            GUILayout.Space(15);
            
            // - - - DONE BUTTON - - -
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            
            if (GUILayout.Button("Done"))
            {
                Close();
            }
            GUILayout.EndHorizontal();
        }

        // For drawing the square colour swatches
        private void DrawColour(string hexData)
        {
            if (ColorUtility.TryParseHtmlString(hexData, out Color parsedColour))
            {
                // Making a 30x30 pixel block in the UI layout
                Rect swatchRect = EditorGUILayout.GetControlRect(GUILayout.Width(30), GUILayout.Height(30));
                
                // Draw the desired colour into that space
                EditorGUI.DrawRect(swatchRect, parsedColour);
                
                // Adding standard button over the top, but making it into a 30x30 square and invisible
                if (GUI.Button(swatchRect, "", GUIStyle.none))
                {
                    chosenColour = parsedColour;
                    FolderCustomiser_Colours.SaveColour(hexData);
                    AddRecentColour(hexData);
                }
            }
        }

        // For drawing horizontal divider lines
        private void DrawSeparator()
        {
            GUILayout.Space(10);
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f, 1.0f)); // Draws a dark grey line
            GUILayout.Space(10);
        }
        
        // When adding colours to the recent colours list, don't include anything that's a part of the default swatches
        private bool IsDefaultColour(string hex)
        {
            for (int row = 0; row < defaultColours.Length; row++)
            {
                for (int col = 0; col < defaultColours[row].Length; col++)
                {
                    // Using OrdinalIgnoreCase because it works against mismatched hex casing!
                    if (defaultColours[row][col].Equals(hex, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        // The core logic for the 'recent colours' system
        private void AddRecentColour(string hex)
        {
            // Ignore any of the default colours.
            if (IsDefaultColour(hex)) return;

            if (recentColours.Contains(hex))
            {
                // Remove so it can be bumped to the front
                recentColours.Remove(hex); 
            }
            
            // Add it to the start of the list
            recentColours.Insert(0, hex); 
            
            // Keep the list trimmed to the max size
            if (recentColours.Count > MAX_RECENTS)
            {
                recentColours.RemoveAt(recentColours.Count - 1);
            }

            SaveRecents();
        }

        private void SaveRecents()
        {
            // Join the entire list of colours into one string to save into the EditorPrefs
            string joined = string.Join(",", recentColours);
            EditorPrefs.SetString(RECENTS_PREF_KEY, joined);
        }

        private void LoadRecents()
        {
            string joined = EditorPrefs.GetString(RECENTS_PREF_KEY, "");
            if (!string.IsNullOrEmpty(joined))
            {
                recentColours = new List<string>(joined.Split(','));
            }
        }
    }
}
