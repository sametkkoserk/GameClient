using Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel;
using Runtime.Modules.Core.Settings.Enum;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Runtime.Modules.Core.Settings.View.AudioSettings
{
  public enum AudioSettingsEvent
  {
    MasterSliderValueChanged,
    MusicSliderValueChanged,
    UISliderValueChanged,
  }
  public class AudioSettingsMediator : EventMediator
  {
    [Inject]
    public AudioSettingsView view { get; set; }
    
    [Inject]
    public IAudioModel audioModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(AudioSettingsEvent.MasterSliderValueChanged, OnMasterVolumeChanged);
      view.dispatcher.AddListener(AudioSettingsEvent.MusicSliderValueChanged, OnMusicVolumeChanged);
      view.dispatcher.AddListener(AudioSettingsEvent.UISliderValueChanged, OnUIVolumeChanged);

      Init();
    }

    private void Init()
    {
      view.masterVolumeSlider.value = audioModel.masterVolume;
      view.musicVolumeSlider.value = audioModel.musicVolume;
      view.uiVolumeSlider.value = audioModel.uiVolume;
    }

    private void OnUIVolumeChanged()
    {
      audioModel.ChangeUISoundVolume(view.uiVolumeSlider.value);
    }

    private void OnMusicVolumeChanged()
    {
      audioModel.ChangeMusicVolume(view.musicVolumeSlider.value);
    }

    private void OnMasterVolumeChanged()
    {
      audioModel.ChangeMasterVolume(view.masterVolumeSlider.value);
    }

    private void SetPlayerPrefs()
    {
      PlayerPrefs.SetFloat(SettingsSaveKey.MasterVolume.ToString(), audioModel.masterVolume);
      PlayerPrefs.SetFloat(SettingsSaveKey.MusicVolume.ToString(), audioModel.musicVolume);
      PlayerPrefs.SetFloat(SettingsSaveKey.UIVolume.ToString(), audioModel.uiVolume);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(AudioSettingsEvent.MasterSliderValueChanged, OnMasterVolumeChanged);
      view.dispatcher.RemoveListener(AudioSettingsEvent.MusicSliderValueChanged, OnMusicVolumeChanged);
      view.dispatcher.RemoveListener(AudioSettingsEvent.UISliderValueChanged, OnUIVolumeChanged);

      SetPlayerPrefs();
    }
  }
}