using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class RemainingTimeProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      TurnVo turnVo = networkManager.GetData<TurnVo>(vo.message);
      
      dispatcher.Dispatch(MainGameEvent.RemainingTime, turnVo);
    }
  }
}