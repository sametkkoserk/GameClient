using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class HandleMapGeneratorProcessor : EventCommand
  {
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      var vo = (MessageReceivedVo)evt.data;
      var message = vo.message;
      var mapGeneratorVo = networkManager.GetData<MapGeneratorVo>(message);

      mainGameModel.cities = mapGeneratorVo.cityVos;

      dispatcher.Dispatch(MainGameEvent.StartGame);
    }
  }
}