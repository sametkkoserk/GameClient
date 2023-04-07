using Runtime.Modules.Core.Audio.Enum;
using Runtime.Modules.Core.PromiseTool;

namespace Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel
{
  public interface IAudioModel
  {
    float masterVolume { get; }
    
    float musicVolume { get; }
    
    float uiVolume { get; }

    void ChangeMasterVolume(float volume);
    
    IPromise InitMusic();

    void PlayMusic(MusicSoundsKey musicSoundsKey);

    void ResumeMusic();
    
    void StopMusic();

    void ChangeMusicVolume(float volume);

    IPromise InitUISounds();

    void PlayUISound(UISoundsKey uiSoundKey);

    void ChangeUISoundVolume(float volume);
  }
}