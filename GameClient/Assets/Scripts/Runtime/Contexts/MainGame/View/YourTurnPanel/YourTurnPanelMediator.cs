using System;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.YourTurnPanel
{
  public class YourTurnPanelMediator : EventMediator
  {
    [Inject]
    public YourTurnPanelView view { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
    }

    private void Start()
    {
      view.totalTime = lobbyModel.lobbyVo.lobbySettingsVo.turnTime;
      view.remainingTime = view.totalTime;
    }

    private void FixedUpdate()
    {
      if (view.remainingTime <= 0f && !view.turnEnded)
      {
        view.turnEnded = true;
        TimeOver();
        return;
      }
      
      view.remainingTime -= Time.deltaTime;
      UpdateSlider();
    }

    private void UpdateSlider()
    {
      view.sliderImage.fillAmount = view.remainingTime / view.totalTime;
      view.timer.text = view.remainingTime.ToString("f0");

      if (view.sliderImage.fillAmount <= 0.25f)
      {
        view.sliderImage.color = Color.red;
      }
    }

    private void TimeOver()
    {
      dispatcher.Dispatch(MainGameEvent.TurnTimeOver);
    }

    public override void OnRemove()
    {
    }
  }
}