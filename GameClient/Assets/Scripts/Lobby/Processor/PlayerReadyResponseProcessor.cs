using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Main.Enum;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Lobby.Processor
{
  public class PlayerReadyResponseProcessor : EventCommand
  {

    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Message message = vo.message;

      ushort inLobbyId = message.GetUShort();
      bool startGame = message.GetBool();
      
      lobbyModel.lobbyVo.clients[inLobbyId].ready=true;
      lobbyModel.lobbyVo.readyCount += 1;
      

      dispatcher.Dispatch(LobbyEvent.PlayerReadyResponse,inLobbyId);
      Debug.Log("player ready responded");
      if (startGame)
      {
        Addressables.LoadSceneAsync(SceneKeys.MainGameScene, LoadSceneMode.Additive);
        dispatcher.Dispatch(LobbyEvent.StartGame);
        
      }
      

    }
  }
}