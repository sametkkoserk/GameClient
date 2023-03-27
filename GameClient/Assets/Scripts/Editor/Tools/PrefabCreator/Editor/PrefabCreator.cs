using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Tools.PrefabCreator.Editor
{
  [Serializable]
  public class PrefabCreator
  {
#if UNITY_EDITOR
    public static void InstantiateObject(string objName, string specialName = null)
    {
      Object gameObject = Resources.Load("Prefab/" + objName);
      
      PrefabUtility.InstantiatePrefab(gameObject, Selection.activeTransform);
      gameObject.name = specialName ?? objName;

      // AsyncOperationHandle<GameObject> instantiateAsync = Addressables.InstantiateAsync(objName, Selection.activeTransform);
      // instantiateAsync.Completed += handle =>
      // {
      //   if (handle.Result == null)
      //     return;
      //
      //   GameObject gameObject = handle.Result;
      //   
      //   gameObject.
      //
      //   gameObject.name = specialName ?? objName;
      // };
    }
    
    [MenuItem("GameObject/PrefabList/MainCamera", false, 3000)]
    public static void MainCamera()
    {
      InstantiateObject(PrefabKey.MainCamera);
    }
    
    [MenuItem("GameObject/PrefabList/StandardText", false, 4000)]
    public static void StandardText()
    {
      InstantiateObject(PrefabKey.StandardText);
    }
    
    [MenuItem("GameObject/PrefabList/ParameterText", false, 4001)]
    public static void ParameterText()
    {
      InstantiateObject(PrefabKey.ParameterText);
    }
    
    [MenuItem("GameObject/PrefabList/StandardTextButton", false, 5000)]
    public static void StandardTextButton()
    {
      InstantiateObject(PrefabKey.StandardButton);
    }
    
    [MenuItem("GameObject/PrefabList/ParameterTextButton", false, 5001)]
    public static void ParameterTextButton()
    {
      InstantiateObject(PrefabKey.ParameterTextButton);
    }
    
    [MenuItem("GameObject/PrefabList/Icon", false, 6000)]
    public static void Icon()
    {
      InstantiateObject(PrefabKey.Icon);
    }
#endif
  }
}