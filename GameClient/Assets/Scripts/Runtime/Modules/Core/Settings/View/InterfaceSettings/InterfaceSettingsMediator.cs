using strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.Settings.View.InterfaceSettings
{
  public class InterfaceSettingsMediator : EventMediator
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