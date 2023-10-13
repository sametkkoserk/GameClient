using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class MiniGameRewardsProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      List<MiniGameResultVo> miniGameResultVos = networkManager.GetData<List<MiniGameResultVo>>(vo.message);
      
      for (int i = 0; i < miniGameResultVos.Count; i++)
      {
        MiniGameResultVo miniGameResultVo = miniGameResultVos.ElementAt(i);
        if (miniGameResultVo.id != lobbyModel.clientVo.id) continue;
        mainGameModel.playerFeaturesVo = miniGameResultVo.playerFeaturesVo;
      }

      mainGameModel.miniGameResultVos = miniGameResultVos;
      dispatcher.Dispatch(MainGameEvent.OpenMiniGameResultPanel);
    }
  }
}