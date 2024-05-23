using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.Settings.View.Settings
{
  public class SettingsPanelView : EventView
  {
    public void OnClosePanel()
    {
      dispatcher.Dispatch(SettingsPanelEvent.ClosePanel);
    }
    public void OnLogout()
    {
      dispatcher.Dispatch(SettingsPanelEvent.Logout);
    }
  }
}