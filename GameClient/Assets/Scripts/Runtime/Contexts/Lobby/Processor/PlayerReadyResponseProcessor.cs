using Riptide;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Lobby.Processor
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