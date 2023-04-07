using strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.Audio.View.SourceCreator
{
  public class AudioSourceCreatorMediator : EventMediator
  {
    [Inject]
    public VIEW view { get; set; }

    public override void OnRegister()
    {
    }

    public override void OnRemove()
    {
    }
  }
}