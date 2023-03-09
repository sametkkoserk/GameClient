using Runtime.Modules.Core.ScreenManager.Enum;
using UnityEditor;

namespace Editor.Tools.SortingLayerCreator.Runtime
{
#if UNITY_EDITOR
  public class CreateLayer : EditorWindow
  {
    [MenuItem("Tools/Sorting Layer/Update Sorting Layers")]
    private static void Init()
    {
      DeleteAllSortingLayers();
      
      string[] layers = System.Enum.GetNames(typeof(LayerKey));
      for (int i = 0; i < layers.Length; i++)
      {
          CreateSortingLayer(layers[i]);
      }
    }

    public static void DeleteAllSortingLayers()
    {
      SerializedObject serializedObject = new(AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset"));
      SerializedProperty sortingLayers = serializedObject.FindProperty("m_SortingLayers");

      sortingLayers.ClearArray();
      
      serializedObject.ApplyModifiedProperties();
    }
    
    /// <summary>It creates new sorting layers. Do not use directly! If you want to create sorting layer, you need to add new key
    /// to LayerKey.cs script in ScreenManager. uses <see cref="LayerKey"/>.</summary>
    /// <param name="layerName">New layer name.</param>
    public static void CreateSortingLayer(string layerName)
    {
      SerializedObject serializedObject = new(AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset"));
      SerializedProperty sortingLayers = serializedObject.FindProperty("m_SortingLayers");

      for (int i = 0; i < sortingLayers.arraySize; i++)
        if (sortingLayers.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue.Equals(layerName))
          return;

      sortingLayers.InsertArrayElementAtIndex(sortingLayers.arraySize);
      SerializedProperty newLayer = sortingLayers.GetArrayElementAtIndex(sortingLayers.arraySize - 1);

      newLayer.FindPropertyRelative("name").stringValue = layerName;
      newLayer.FindPropertyRelative("uniqueID").intValue = layerName.GetHashCode(); /* some unique number */

      serializedObject.ApplyModifiedProperties();
    }
  }
#endif
}