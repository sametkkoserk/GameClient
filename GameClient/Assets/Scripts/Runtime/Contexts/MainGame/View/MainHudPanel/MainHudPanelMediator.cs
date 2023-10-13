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
      view.dispatcher.AddListener(MainHudPanelEvent.ChangeSizeOfPlayerList, OnHideOrShowOfPlayerList);
      
      dispatcher.AddListener(MainGameEvent.RemainingTimeMainHud, OnRemainingTime);
      dispatcher.AddListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.AddListener(MainGameEvent.StopTimer, OnStopTimer);
      
      Init();
    }

    private void Init()
    {
      view.sliderImage.gameObject.SetActive(false);
      view.timer.gameObject.SetActive(false);
      
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
    
    private void OnNextTurn(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;
      
      view.sliderImage.gameObject.SetActive(true);
      view.timer.gameObject.SetActive(true);

      view.sliderImage.color = mainHudTurnVo.color.ToColor();

      view.timerSlideTween?.Kill();
      view.sliderImage.fillAmount = 100;
      view.timerSlideTween  = view.sliderImage.DOFillAmount(0, view.totalTime).SetEase(Ease.Linear);
      view.timer.text = view.totalTime.ToString("f0");
    }

    public void OnStopTimer()
    {
      view.timerSlideTween?.Kill();
      
      view.sliderImage.gameObject.SetActive(false);
      view.timer.gameObject.SetActive(false);
    }

    private void OnHideOrShowOfPlayerList(IEvent payload)
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
      view.dispatcher.RemoveListener(MainHudPanelEvent.ChangeSizeOfPlayerList, OnHideOrShowOfPlayerList);
      
      dispatcher.RemoveListener(MainGameEvent.RemainingTimeMainHud, OnRemainingTime);
      dispatcher.RemoveListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.RemoveListener(MainGameEvent.StopTimer, OnStopTimer);
    }
  }
}