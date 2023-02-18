using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

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
        dispatcher.Dispatch(LobbyEvent.StartGame);
        
      }
      

    }
  }
}