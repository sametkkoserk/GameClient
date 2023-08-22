using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.View.Notification.Vo;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.Localization.Enum;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Processor
{
  public class LobbyIsClosedProcessor : EventCommand
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    public override void Execute()
    {
      MessageReceivedVo unnecessaryData = (MessageReceivedVo)evt.data;

      NotificationVo vo = new()
      {
        delayTime = 3,
        headerKey = TranslateKeys.NotificationError,
        contentKey = TranslateKeys.NotificationLobbyIsInactive
      };
      
      crossDispatcher.Dispatch(MainEvent.OpenNotificationPanel, vo);
      
      dispatcher.Dispatch(LobbyEvent.RefreshLobbyList);
    }
  }
}