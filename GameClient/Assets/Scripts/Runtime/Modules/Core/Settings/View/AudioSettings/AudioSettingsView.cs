using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Settings.View.AudioSettings
{
  public class AudioSettingsView : EventView
  {
    public Slider masterVolumeSlider;

    public void OnMasterVolumeChanged()
    {
      dispatcher.Dispatch(AudioSettingsEvent.MasterSliderValueChanged);
    }
    
    public Slider musicVolumeSlider;
    
    public void OnMusicVolumeChanged()
    {
      dispatcher.Dispatch(AudioSettingsEvent.MusicSliderValueChanged);
    }
    
    public Slider uiVolumeSlider;

    public void OnUIVolumeChanged()
    {
      dispatcher.Dispatch(AudioSettingsEvent.UISliderValueChanged);
    }
  }
}