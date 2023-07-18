#if UNITY_EDITOR
using Runtime.Modules.Core.Icon.View;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools.IconSetter.Editor
{
  [CustomEditor(typeof(IconView))]
  public class SetIcon : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();

      IconView iconView = (IconView)target;

      if (GUILayout.Button(new GUIContent("Save", "Change Icon On Inspector!"), EditorStyles.miniButton)) iconView.SetFromInspector();
    }
  }
}
#endif