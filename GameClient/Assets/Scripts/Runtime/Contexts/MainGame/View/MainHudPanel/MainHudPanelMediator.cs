using System.Linq;
using DG.Tweening;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.View.MainHudPanel.Item;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.Icon.Enum;
using Runtime.Modules.Core.Icon.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainHudPanel
{
  public enum MainHudPanelEvent
  {
    ChangeSizeOfPlayerList
  }
  public class MainHudPanelMediator : EventMediator
  {
    [Inject]
    public MainHudPanelView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(MainHudPanelEvent.ChangeSizeOfPlayerList, OnChangeSizeOfPlayerList);
      
      dispatcher.AddListener(MainGameEvent.RemainingTimeMainHud, OnRemainingTime);
      dispatcher.AddListener(MainGameEvent.NextTurnMainHud, OnNexTurn);
      
      Init();
    }

    private void Init()
    {
      view.totalTime = lobbyModel.lobbyVo.lobbySettingsVo.turnTime;

      for (int i = 0; i < lobbyModel.lobbyVo.clients.Count; i++)
      {
        int count = i;
        
        GameObject joinLobbyPanelItem = Instantiate(view.playerListItem, view.playerListContainer);
        MainHudPanelPlayerItemView behaviour = joinLobbyPanelItem.GetComponent<MainHudPanelPlayerItemView>();
        ClientVo client = lobbyModel.lobbyVo.clients.ElementAt(count).Value;

        MainHudPlayerVo mainHudPlayerVo = new()
        {
          playerColor = client.playerColor.ToColor(),
          username = client.userName,
          id = client.id,
          isMe = client.id == lobbyModel.clientVo.id
        };
        
        behaviour.Init(mainHudPlayerVo);
      }
    }

    private void OnRemainingTime(IEvent payload)
    {
      int remainingTime = (int)payload.data;
      
      view.timer.text = remainingTime.ToString("f0");
    }
    
    private void OnNexTurn(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;

      view.sliderImage.color = mainHudTurnVo.playerColor.ToColor();

      view.sliderImage.fillAmount = 100;
      view.sliderImage.DOFillAmount(0, view.totalTime).SetEase(Ease.Linear);
      view.timer.text = view.totalTime.ToString("f0");
    }

    private void OnChangeSizeOfPlayerList(IEvent payload)
    {
      bool data = (bool)payload.data;
      dispatcher.Dispatch(MainGameEvent.ChangeSizeOfPlayerList, data);

      IconVo iconVo = new()
      {
        iconKey = view.showPlayerList ? IconKey.ShowEyeIcon : IconKey.HideEyeIcon
      };
      view.showHidePlayerListButtonIcon.Init(iconVo);

      view.showPlayerList = !view.showPlayerList;
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MainHudPanelEvent.ChangeSizeOfPlayerList, OnChangeSizeOfPlayerList);
      
      dispatcher.RemoveListener(MainGameEvent.RemainingTimeMainHud, OnRemainingTime);
      dispatcher.RemoveListener(MainGameEvent.NextTurnMainHud, OnNexTurn);
    }
  }
}