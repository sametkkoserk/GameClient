using DG.Tweening;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainHudPanel.Item
{
  public class MainHudPanelPlayerItemMediator : EventMediator
  {
    [Inject]
    public MainHudPanelPlayerItemView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.ChangeSizeOfPlayerList, OnChangeSizeOfPlayerList);
      dispatcher.AddListener(MainGameEvent.NextTurnMainHud, OnTurnChanges);
    }

    private void OnTurnChanges(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;
      
      if (view.id == mainHudTurnVo.id)
      {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1), 1f).SetEase(Ease.OutQuart);
      }
      else
      {
        transform.DOScale(new Vector3(1, 1, 1), 1f).SetEase(Ease.Linear);
      }
    }

    private void OnChangeSizeOfPlayerList(IEvent payload)
    {
      bool data = (bool)payload.data;

      int value = data ? 0 : 1;

      view.background.DOScale(new Vector3(value, 1, 1), 2f);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.ChangeSizeOfPlayerList, OnChangeSizeOfPlayerList);
      dispatcher.RemoveListener(MainGameEvent.NextTurnMainHud, OnTurnChanges);
    }
  }
}