using System.Collections;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.NextTurnNotificationPanel
{
  public class NextTurnNotificationPanelMediator : EventMediator
  {
    [Inject]
    public NextTurnNotificationPanelView panelView { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.NextTurnNotificationPanel, OnSetInfosTurnOwner);
    }

    public void OnSetInfosTurnOwner(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;

      panelView.canvasGroup.alpha = 1f;
      panelView.canvasGroup.blocksRaycasts = true;
        
      panelView.nameOfTurnOwner.text = mainHudTurnVo.playerUsername;

      panelView.notificationPanelVo = mainHudTurnVo;
      
      StartCoroutine(AfterOpeningPanel());
    }

    private IEnumerator AfterOpeningPanel()
    {
      yield return new WaitForSecondsRealtime(1f);

      panelView.canvasGroup.alpha = 0f;
      panelView.canvasGroup.blocksRaycasts = false;
      
      dispatcher.Dispatch(MainGameEvent.NextTurnMainHud, panelView.notificationPanelVo);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.NextTurnNotificationPanel, OnSetInfosTurnOwner);
    }
  }
}