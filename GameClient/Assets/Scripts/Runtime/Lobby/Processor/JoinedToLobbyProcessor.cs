using System.Collections.Generic;
using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Vo;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
{
  public class JoinedToLobbyProcessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Message message = vo.message;

      LobbyVo lobbyVo = new LobbyVo();
      lobbyVo.lobbyId = message.GetUShort();
      lobbyVo.lobbyName = message.GetString();
      lobbyVo.isPrivate = message.GetBool();
      lobbyVo.leaderId = message.GetUShort();
      lobbyVo.playerCount = message.GetUShort();
      lobbyVo.maxPlayerCount = message.GetUShort();
      lobbyModel.inLobbyId = message.GetUShort();
      lobbyVo.clients = new Dictionary<ushort, ClientVo>();
      for (int i = 0; i < lobbyVo.playerCount; i++)
      {
        ClientVo clientVo = new ClientVo();
        clientVo.id = message.GetUShort();
        clientVo.inLobbyId = message.GetUShort();
        //clientVo.userName = message.GetString();
        clientVo.colorId = message.GetUShort();
        lobbyVo.clients[clientVo.inLobbyId]=clientVo;
      }
      
      lobbyModel.lobbyVo = lobbyVo;

      dispatcher.Dispatch(LobbyEvent.ToLobbyManagerPanel);

    }
  }
}