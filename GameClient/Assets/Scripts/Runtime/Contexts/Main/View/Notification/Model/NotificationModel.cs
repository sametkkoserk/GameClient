using Runtime.Contexts.Main.View.Notification.Vo;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.View.Notification.Model
{
  public class NotificationModel : INotificationModel
  {
    public NotificationVo notificationVo { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      notificationVo = new NotificationVo();
    }
  }
}