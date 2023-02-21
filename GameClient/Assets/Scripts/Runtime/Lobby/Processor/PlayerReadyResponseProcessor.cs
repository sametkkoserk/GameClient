using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Main.Enum;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Lobby.Processor
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