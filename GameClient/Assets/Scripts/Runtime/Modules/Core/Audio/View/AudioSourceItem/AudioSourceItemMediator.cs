using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.Audio.Enum;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.Audio.View.AudioSourceItem
{
  public class AudioSourceItemMediator : EventMediator
  {
    [Inject]
    public AudioSourceItemView view { get; set; }

    public override void OnRegister()
    {
      switch (view.soundType)
      {
        case SoundTypes.Music:
          dispatcher.AddListener(MainEvent.PlayMusic, OnPlayMusic);
          dispatcher.AddListener(MainEvent.StopMusic, OnStopMusic);
          dispatcher.AddListener(MainEvent.ResumeMusic, OnResumeMusic);
          dispatcher.AddListener(MainEvent.ChangeMusicVolume, OnChangeMusicVolume);
          break;
        case SoundTypes.UI:
          dispatcher.AddListener(MainEvent.PlayUISound, OnPlayUISound);
          dispatcher.AddListener(MainEvent.ChangeUISoundsVolume, OnChangeUIVolume);
          break;
      }
    }

    #region Music

    private void OnPlayMusic(IEvent payload)
    {
      AudioClip audioClip = (AudioClip)payload.data;

      view.audioSource.clip = audioClip;
      view.audioSource.Play();
    }
    
    private void OnResumeMusic()
    {
      view.audioSource.UnPause();
    }
    
    private void OnStopMusic()
    {
      view.audioSource.Pause();
    }

    private void OnChangeMusicVolume(IEvent payload)
    {
      float volume = (float)payload.data;
      view.audioSource.volume = volume;
    }
    #endregion

    #region UI
    private void OnPlayUISound(IEvent payload)
    {
      AudioClip audioClip = (AudioClip)payload.data;

      view.audioSource.clip = audioClip;
      view.audioSource.Play();
    }
    
    private void OnChangeUIVolume(IEvent payload)
    {
      float volume = (float)payload.data;
      view.audioSource.volume = volume;
    }

    #endregion
    public override void OnRemove()
    {
      switch (view.soundType)
      {
        case SoundTypes.Music:
          dispatcher.RemoveListener(MainEvent.PlayMusic, OnPlayMusic);
          dispatcher.RemoveListener(MainEvent.StopMusic, OnStopMusic);
          dispatcher.RemoveListener(MainEvent.ResumeMusic, OnResumeMusic);
          dispatcher.RemoveListener(MainEvent.ChangeMusicVolume, OnChangeMusicVolume);
          break;
        case SoundTypes.UI:
          dispatcher.RemoveListener(MainEvent.PlayUISound, OnPlayUISound);
          dispatcher.RemoveListener(MainEvent.ChangeUISoundsVolume, OnChangeUIVolume);
          break;
      }
    }
  }
}