using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.MiniGameStatsPanel.Item
{
  public class MiniGameResultPanelItemView : EventView
  {
    public Image playerColor;

    public TextMeshProUGUI playerName;

    public TextMeshProUGUI playerArrangement;

    public TextMeshProUGUI playerRewards;

    public void SetItem(MiniGameResultVo vo)
    {
      dispatcher.Dispatch(MiniGameResultEvent.SetItems, vo);
    }
  }
}