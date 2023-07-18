using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;

namespace Runtime.Contexts.Main.View.Popup
{
  public class PopupPanelView : EventView
  {
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI contentText;

    public void OnClosePanel()
    {
      dispatcher.Dispatch(PopupPanelEvent.ClosePanel);
    }
  }
}