using Lobby.Model.LobbyModel;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Lobby.Processor
{
  public class JoinedToLobbyProcessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Message message = vo.message;

      lobbyModel.lobbyId = message.GetUShort();
      lobbyModel.lobbyName = message.GetString();
      lobbyModel.isPrivate = message.GetBool();
      lobbyModel.leaderId = message.GetUShort();
      Debug.Log(lobbyModel.lobbyId);
      Debug.Log(lobbyModel.lobbyName);

    }
  }
}