using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class FortifyResultProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;

      FortifyResultVo fortifyResultVo = networkManager.GetData<FortifyResultVo>(vo.message);

      mainGameModel.cities[fortifyResultVo.sourceCity.ID] = fortifyResultVo.sourceCity;
      mainGameModel.cities[fortifyResultVo.targetCity.ID] = fortifyResultVo.targetCity;
      
      dispatcher.Dispatch(MainGameEvent.FortifyResult, fortifyResultVo);
      dispatcher.Dispatch(MainGameEvent.UpdateDetailsPanel);
    }
  }
}