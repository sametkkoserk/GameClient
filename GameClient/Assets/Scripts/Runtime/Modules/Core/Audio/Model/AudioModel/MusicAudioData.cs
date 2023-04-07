using System.Collections.Generic;
using System.Linq;
using Runtime.Modules.Core.Audio.Enum;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.Audio.Model.AudioModel
{
  [CreateAssetMenu(menuName = "Tools/Audio/CreateMusic", fileName = "MusicSoundsSettings")]
  public class MusicAudioData : ScriptableObject
  {
    public List<AudioClip> musicSounds;

#if UNITY_EDITOR

    [MenuItem("Tools/Audio/Remove Repetitive Musics")]
    public static void UpdateMusics()
    {
      AsyncOperationHandle<MusicAudioData> musicAudioData = Addressables.LoadAssetAsync<MusicAudioData>("MusicSoundsSettings");
    
      musicAudioData.WaitForCompletion();
      MusicAudioData data = musicAudioData.Result;
    
      data.musicSounds = data.musicSounds.Distinct().ToList();

      
      List<AudioClip> newList = new();
    
      string[] keys = System.Enum.GetNames(typeof(MusicSoundsKey));
    
      for (int i = 0; i < keys.Length; i++)
      {
        newList.AddRange(data.musicSounds.Where(t => keys[i] == t.name));
      }
    
      data.musicSounds = newList;
    }
#endif
  }
}