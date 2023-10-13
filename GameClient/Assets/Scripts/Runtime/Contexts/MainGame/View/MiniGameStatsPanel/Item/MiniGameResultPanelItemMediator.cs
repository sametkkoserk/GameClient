using System.Linq;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MiniGameStatsPanel.Item
{
  public enum MiniGameResultEvent
  {
    SetItems
  }
  public class MiniGameResultPanelItemMediator : EventMediator
  {
    [Inject]
    public MiniGameResultPanelItemView view { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(MiniGameResultEvent.SetItems, OnSetItems);
    }

    public void OnSetItems(IEvent payload)
    {
      MiniGameResultVo miniGameResultVo = (MiniGameResultVo)payload.data;

      ClientVo clientVo =lobbyModel.lobbyVo.clients[miniGameResultVo.id];
      view.playerColor.color = clientVo.playerColor.ToColor();
      view.playerName.text = clientVo.userName;

      for (int i = 0; i < miniGameResultVo.playerRewards.Count; i++)
      {
        int count = i;
        view.playerRewards.text += "- " + miniGameResultVo.playerRewards.ElementAt(count) + "\n";
      }

      switch (miniGameResultVo.playerArrangement)
      {
        case 1:
          view.playerArrangement.text = 1 + "<size=24>ST</size>";
          view.playerArrangement.color = new Color(1, 0.75f, 0);
          break;
        case 2:
          view.playerArrangement.text = 2 + "<size=24>ND</size>";
          view.playerArrangement.color = new Color(0.8f, 0.8f, 0.8f);
          break;
        case 3:
          view.playerArrangement.text = 3 + "<size=24>RD</size>";
          view.playerArrangement.color = new Color(0.8f, 0.5f, 0.25f);
          break;
        default:
          view.playerArrangement.text = miniGameResultVo.playerArrangement + "<size=24>TH</size>";
          view.playerArrangement.color = new Color(0.5f, 0.5f, 0.5f);
          break;
      }
    }
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MiniGameResultEvent.SetItems, OnSetItems);
    }
  }
}