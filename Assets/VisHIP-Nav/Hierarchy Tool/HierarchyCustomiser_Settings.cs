using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

[DisallowMultipleComponent] // I don't want this being added twice to one object
public class HierarchyCustomiser_Settings : MonoBehaviour
{
     public Color backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0f);
     public Color textColor = Color.white;
     
     public bool isBold = false;
     public bool isItalic = false;
     
     public TextAnchor alignmment = TextAnchor.MiddleLeft;
     
     public bool uppercase = false;
     
     #if UNITY_EDITOR
     private void Reset()
     {
          // When the component is attached, immediately push it to the very top of the inspector
          while (ComponentUtility.MoveComponentUp(this)){}
     }
     #endif
}
