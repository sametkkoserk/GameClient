using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Lobby.Processor
{
  public class PlayerReadyResponseProcessor : EventCommand
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      PlayerReadyResponseVo playerReadyResponseVo = networkManager.GetData<PlayerReadyResponseVo>(vo.message);

      if (lobbyModel.lobbyVo.lobbyId != playerReadyResponseVo.lobbyId)
        return;

      lobbyModel.lobbyVo.clients[playerReadyResponseVo.inLobbyId].ready = true;
      lobbyModel.lobbyVo.readyCount += 1;

      dispatcher.Dispatch(LobbyEvent.PlayerReadyResponse, playerReadyResponseVo.inLobbyId);

      Debug.Log("player ready responded");

      if (!playerReadyResponseVo.startGame) return;

      crossDispatcher.Dispatch(MainEvent.CloseMainSceneCamera);

      Addressables.LoadSceneAsync(SceneKeys.MainGameScene, LoadSceneMode.Additive);
      screenManagerModel.CloseScenePanels(SceneKey.Lobby);
    }
  }
}