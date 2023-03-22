using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;

namespace Runtime.Modules.Core.Settings.View.Settings
{
  public class SettingsPanelView : EventView
  {
    public TextMeshProUGUI textMeshProUGUI;
    public void OnClosePanel()
    {
      dispatcher.Dispatch(SettingsPanelEvent.ClosePanel);
    }
  }
}