using System.Threading.Tasks;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainGameNotificationPanel
{
  public class MainGameNotificationPanelMediator : EventMediator
  {
    [Inject]
    public MainGameNotificationPanelView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.NotificationPanel, OnSetNotificationPanel);

      Init();
    }

    private void Init()
    {
      dispatcher.Dispatch(MainGameEvent.StopTimer);

      view.canvasGroup.alpha = 0f;
      view.canvasGroup.blocksRaycasts = false;
    }

    public void OnSetNotificationPanel(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;

      SetNotificationPanel(mainHudTurnVo);
    }

    public async Task SetNotificationPanel(MainHudTurnVo mainHudTurnVo)
    {
      if (mainHudTurnVo.panelTypeKey == NotificationPanelTypeKey.NextTurn)
      {
        view.notificationPanelVo = mainHudTurnVo;
        OnFillPanelContents();
        
        dispatcher.Dispatch(MainGameEvent.StopTimer);

        await WaitAsyncOperations(mainHudTurnVo.time);

        dispatcher.Dispatch(MainGameEvent.NextTurnMainHud, view.notificationPanelVo);

        return;
      }

      view.notificationPanelVo = mainHudTurnVo;
      OnFillPanelContents();
      await WaitAsyncOperations(mainHudTurnVo.time);
    }

    public void OnFillPanelContents()
    {
      view.title.text = view.notificationPanelVo.title;
      SetImages(view.notificationPanelVo.color.ToColor());
    }

    private MainHudTurnVo SetVo(string title, PlayerColorVo color, NotificationPanelTypeKey notificationPanelTypeKey, float time)
    {
      MainHudTurnVo mainHudTurnVo = new()
      {
        title = title,
        color = color,
        panelTypeKey = notificationPanelTypeKey,
        time = time
      };

      return mainHudTurnVo;
    }

    private void SetImages(Color color)
    {
      for (int i = 0; i < view.colorImages.Count; i++)
      {
        view.colorImages[i].color = color;
      }
    }

    public async Task WaitAsyncOperations(float sec)
    {
      view.canvasGroup.alpha = 1f;
      view.canvasGroup.blocksRaycasts = true;

      await Task.Delay((int)(sec * 1000));

      view.canvasGroup.alpha = 0f;
      view.canvasGroup.blocksRaycasts = false;
    }


    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.NotificationPanel, OnSetNotificationPanel);
    }
  }
}