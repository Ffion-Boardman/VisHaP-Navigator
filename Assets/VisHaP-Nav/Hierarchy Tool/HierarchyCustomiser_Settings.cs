using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

[DisallowMultipleComponent] // I don't want this being added twice to one object
public class HierarchyCustomiser_Settings : MonoBehaviour
{
     [ColorUsage(false)] // Means users cannot access the alpha channel
     public Color backgroundColor = new Color(0.2196079f, 0.2196079f, 0.2196079f, 1f); // Use the default Unity background panel colour
     [ColorUsage(false)]
     public Color textColor = new Color(0.7921569f, 0.7921569f, 0.7921569f, 1f); // Use the default Unity text colour

     public bool isBold = false;
     public bool isItalic = false;
     
     public TextAnchor alignment = TextAnchor.MiddleLeft;
     
     public bool uppercase = false;
     
     #if UNITY_EDITOR
     private void Reset()
     {
          // When the component is attached, immediately push it to the very top of the inspector
          while (ComponentUtility.MoveComponentUp(this)){}
     }
     #endif
}
