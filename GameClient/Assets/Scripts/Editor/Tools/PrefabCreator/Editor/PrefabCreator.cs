using System;
using Runtime.Contexts.Lobby.Enum;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Editor.Tools.PrefabCreator.Editor
{
  [Serializable]
  public class PrefabCreator
  {
#if UNITY_EDITOR
    public static void InstantiateObject(string objName, string specialName = null)
    {
      AsyncOperationHandle<GameObject> instantiateAsync = Addressables.InstantiateAsync(objName, Selection.activeTransform);
      instantiateAsync.Completed += handle =>
      {
        if (handle.Result == null)
          return;

        GameObject gameObject = handle.Result;

        gameObject.name = specialName ?? objName;
      };
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
#endif
  }
}