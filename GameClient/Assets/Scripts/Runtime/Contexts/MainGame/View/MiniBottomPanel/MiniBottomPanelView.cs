using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.MiniBottomPanel
{
  public class MiniBottomPanelView : EventView
  {
    public Image[] images;

    public TextMeshProUGUI playerName;

    [FormerlySerializedAs("stage")]
    public TextMeshProUGUI stateText;

    public Button operationButton;

    public void Pass()
    {
      dispatcher.Dispatch(MiniBottomPanelEvent.Pass);
    }
  }
}