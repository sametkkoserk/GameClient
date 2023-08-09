using System;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Main.View.Notification.Model;
using Runtime.Contexts.Main.View.Notification.Vo;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.Notification.View
{
  public enum NotificationPanelEvent
  {
    ClosePanel
  }
  public class NotificationPanelMediator : EventMediator
  {
    [Inject]
    public NotificationPanelView view { get; set; }
    
    [Inject]
    public INotificationModel notificationModel { get; set; }
    
    [Inject]
    public ILocalizationModel localizationModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(NotificationPanelEvent.ClosePanel, OnClose);
      
      OnShow();
    }

    private void Update()
    {
      view.layoutElement.enabled = Math.Max(view.headerText.preferredWidth, view.contentText.preferredWidth) >= view.layoutElement.preferredWidth;
    }
    
    private void OnShow()
    {
      DebugX.Log(DebugKey.Notification, "Notification Mediator OnShow 1");

      if (view.headerText == null || view.contentText == null)
        return;

      NotificationVo vo = notificationModel.notificationVo;

      if (string.IsNullOrEmpty(vo.headerKey.ToString()))
        view.headerText.gameObject.SetActive(false);
      
      if (string.IsNullOrEmpty(vo.contentKey.ToString()))
        view.contentText.gameObject.SetActive(false);

      view.headerText.text = localizationModel.GetText(TableKey.Notification, vo.headerKey);
      view.contentText.text = localizationModel.GetText(TableKey.Notification, vo.contentKey);

      DebugX.Log(DebugKey.Notification, "Notification Mediator OnShow 2");
      
      Hide(1f);
    }

    private void Hide(float time)
    {
      view.FadeAnimation(time, 0, notificationModel.notificationVo.delayTime);
      DebugX.Log(DebugKey.Notification, "Notification Mediator Hide");
    }
    
    private void OnClose()
    {
      notificationModel.notificationVo.delayTime = 0f;
      Hide(0.5f);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(NotificationPanelEvent.ClosePanel, OnClose);
    }
  }
}