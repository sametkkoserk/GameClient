using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Processor
{
  public class RegisterAcceptedProcessor : EventCommand
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      PlayerRegisterInfoVo playerRegisterInfoVo = networkManager.GetData<PlayerRegisterInfoVo>(vo.message);

      playerModel.playerRegisterInfoVo = playerRegisterInfoVo;
      
      screenManagerModel.CloseAllPanels();
      crossDispatcher.Dispatch(LobbyEvent.LoginOrRegisterCompletedSuccessfully);
    }
  }
}