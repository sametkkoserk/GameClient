using System;
using System.Collections.Generic;
using Runtime.Modules.Core.Audio.Vo;
using UnityEngine;

namespace Runtime.Modules.Core.Audio.Model.AudioModel
{
  [CreateAssetMenu(menuName = "Tools/Audio/CreateMusic", fileName = "MusicSoundsSettings")]
  [Serializable]
  public class AudioDataMusic : ScriptableObject
  {
      public List<MusicSoundsVo> musicSounds;


    [CreateAssetMenu(menuName = "Tools/Audio/CreateUI", fileName = "UISoundsSettings")]
    [Serializable]
    public class AudioDataUI : ScriptableObject
    {
      // public List<CursorVo> list;
    }


    [CreateAssetMenu(menuName = "Tools/Audio/CreateEffect", fileName = "EffectSoundsSettings")]
    [Serializable]
    public class AudioDataEffect : ScriptableObject
    {
      // public List<CursorVo> list;
    }
  }
}