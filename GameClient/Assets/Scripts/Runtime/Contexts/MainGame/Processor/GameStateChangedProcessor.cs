using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class GameStateChangedProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      GameStateKey gameStateKey = networkManager.GetData<GameStateKey>(vo.message);

      mainGameModel.gameStateKey = gameStateKey;

      dispatcher.Dispatch(MainGameEvent.GameStateChanged);
      
      DebugX.Log(DebugKey.MainGame,"Game State Changed: " + mainGameModel.gameStateKey);
    }
  }
}