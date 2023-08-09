using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.View.Notification.Model;
using Runtime.Contexts.Main.View.Notification.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class OpenNotificationPanelCommand : EventCommand
  {
    [Inject]
    public INotificationModel notificationModel { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      NotificationVo vo = (NotificationVo)evt.data;

      notificationModel.notificationVo = vo;
      
      screenManagerModel.OpenPanel(MainPanelKey.NotificationPanel, SceneKey.Main, LayerKey.NotificationLayer, PanelMode.Destroy, PanelType.PopupPanel);
    }
  }
}