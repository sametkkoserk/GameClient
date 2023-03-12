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
    public static void InstantiateObject(string objName)
    {
      AsyncOperationHandle<GameObject> instantiateAsync = Addressables.InstantiateAsync(objName, Selection.activeTransform);
      instantiateAsync.Completed += handle =>
      {
        if (handle.Result == null)
          return;

        GameObject gameObject = handle.Result;
        gameObject.name = objName;
      };
    }
    
    [MenuItem("GameObject/PrefabList/MainCamera", false, 3000)]
    public static void MainCamera()
    {
      InstantiateObject(PrefabKey.MainCamera);
    }
#endif
  }
}