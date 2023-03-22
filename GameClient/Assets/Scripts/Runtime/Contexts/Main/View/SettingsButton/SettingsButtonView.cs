using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.SettingsButton
{
  public class SettingsButtonView : EventView
  {
    public void OnOpenSettingsPanel()
    {
      dispatcher.Dispatch(SettingsButtonEvent.OpenSettingsPanel);
    }
  }
}