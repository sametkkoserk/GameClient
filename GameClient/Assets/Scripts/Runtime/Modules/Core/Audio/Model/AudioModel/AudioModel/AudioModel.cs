using System;
using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.Audio.Enum;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.PromiseTool;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel
{
  public class AudioModel : IAudioModel
  {
    [Inject]
    public IBundleModel bundleModel { get; set; }

    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

    public static IAudioModel instance;

    private List<AudioClip> musicSounds;

    private List<AudioClip> UISounds;

    public float masterVolume { get; set; }

    public float musicVolume { get; set; }

    public float uiVolume { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      musicSounds = new List<AudioClip>();
      UISounds = new List<AudioClip>();

      masterVolume = 1;
      musicVolume = 1;
      uiVolume = 1;
    }

    public void ChangeMasterVolume(float volume)
    {
      masterVolume = volume;
      
      ChangeMusicVolume(musicVolume);
      ChangeUISoundVolume(uiVolume);
    }

    #region Music

    public IPromise InitMusic()
    {
      Promise promise = new();

      bundleModel.LoadAssetAsync<MusicAudioData>("MusicSoundsSettings").Then(data =>
      {
        foreach (AudioClip clip in data.musicSounds)
          musicSounds.Add(clip);

        instance = this;


        promise.Resolve();
      }).Catch(promise.Reject);

      return promise;
    }

    public void PlayMusic(MusicSoundsKey musicSoundsKey)
    {
      int index = (int)musicSoundsKey;
      if (index < musicSounds.Count)
      {
        if (musicSoundsKey.ToString() == musicSounds[index].name)
          crossDispatcher.Dispatch(MainEvent.PlayMusic, musicSounds[index]);
        else
          DebugX.Log(DebugKey.Audio, "Keys in the same index do not match!", LogKey.Warning);
      }
      else
       DebugX.Log(DebugKey.Audio, "Index is out of range!", LogKey.Error);
    }

    public void ResumeMusic()
    {
      crossDispatcher.Dispatch(MainEvent.ResumeMusic);
    }

    public void StopMusic()
    {
      crossDispatcher.Dispatch(MainEvent.StopMusic);
    }

    public void ChangeMusicVolume(float volume)
    {
      musicVolume = volume;
      crossDispatcher.Dispatch(MainEvent.ChangeMusicVolume, musicVolume * masterVolume);
    }

    #endregion

    #region UI

    public IPromise InitUISounds()
    {
      Promise promise = new();

      bundleModel.LoadAssetAsync<UIAudioData>("UISoundsSettings").Then(data =>
      {
        foreach (AudioClip clip in data.uiSounds)
          UISounds.Add(clip);

        instance = this;

        promise.Resolve();
      }).Catch(promise.Reject);

      return promise;
    }

    public void PlayUISound(UISoundsKey uiSoundKey)
    {
      int index = (int)uiSoundKey;
      if (index < UISounds.Count)
      {
        if (uiSoundKey.ToString() == UISounds[index].name)
          crossDispatcher.Dispatch(MainEvent.PlayUISound, UISounds[index]);
        else
          DebugX.Log(DebugKey.Audio, "Keys in the same index do not match!", LogKey.Warning);
      }
      else
        DebugX.Log(DebugKey.Audio, "Index is out of range!", LogKey.Error);
    }

    public void ChangeUISoundVolume(float volume)
    {
      uiVolume = volume;
      crossDispatcher.Dispatch(MainEvent.ChangeUISoundsVolume, uiVolume * masterVolume);
    }

    #endregion
  }
}