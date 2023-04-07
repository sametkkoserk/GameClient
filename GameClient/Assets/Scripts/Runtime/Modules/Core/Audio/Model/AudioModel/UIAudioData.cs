using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Modules.Core.Audio.Enum;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.Audio.Model.AudioModel
{
  [CreateAssetMenu(menuName = "Tools/Audio/CreateUI", fileName = "UISoundsSettings")]
  [Serializable]
  public class UIAudioData : ScriptableObject
  {
    public List<AudioClip> uiSounds;
      
#if UNITY_EDITOR
      
      [MenuItem("Tools/Audio/Remove Repetitive UI Sounds")]

      public static void RemoveDistinctMusic()
      {
        AsyncOperationHandle<UIAudioData> musicAudioData = Addressables.LoadAssetAsync<UIAudioData>("UISoundsSettings");

        musicAudioData.WaitForCompletion();
        UIAudioData data = musicAudioData.Result;

        data.uiSounds = data.uiSounds.Distinct().ToList();
        
        List<AudioClip> newList = new();
    
        string[] keys = System.Enum.GetNames(typeof(UISoundsKey));
    
        for (int i = 0; i < keys.Length; i++)
        {
          newList.AddRange(data.uiSounds.Where(t => keys[i] == t.name));
        }
    
        data.uiSounds = newList;
      }
#endif
  }
}