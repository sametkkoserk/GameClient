using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class ClaimedCityProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      CityVo cityVo = networkManager.GetData<CityVo>(vo.message);

      mainGameModel.cities[cityVo.ID] = cityVo;

      dispatcher.Dispatch(MainGameEvent.ClaimedCity, cityVo);
      dispatcher.Dispatch(MainGameEvent.UpdateDetailsPanel);
      
      DebugX.Log(DebugKey.MainGame,"City Claimed: " + cityVo.ID);
    }
  }
}