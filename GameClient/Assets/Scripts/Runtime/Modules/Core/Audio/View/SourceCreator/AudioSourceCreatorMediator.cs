using Runtime.Modules.Core.Audio.Enum;
using Runtime.Modules.Core.Audio.View.AudioSourceItem;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.Audio.View.SourceCreator
{
  public class AudioSourceCreatorMediator : EventMediator
  {
    [Inject]
    public AudioSourceCreatorView view { get; set; }

    public override void OnRegister()
    {
      Init();
    }

    private void Init()
    {
      string[] soundTypes = System.Enum.GetNames(typeof(SoundTypes));

      for (int i = 0; i < soundTypes.Length; i++)
      {
        GameObject item = Instantiate(view.audioSourceItem, transform);

        AudioSourceItemView itemView = item.GetComponent<AudioSourceItemView>();

        itemView.soundType = (SoundTypes) i;
      }
    }

    public override void OnRemove()
    {
    }
  }
}