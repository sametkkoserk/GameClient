using System.Collections.Generic;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.Vo;
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

      LobbyVo lobbyVo = new LobbyVo();
      lobbyVo.lobbyId = message.GetUShort();
      lobbyVo.lobbyName = message.GetString();
      lobbyVo.isPrivate = message.GetBool();
      lobbyVo.leaderId = message.GetUShort();
      lobbyVo.playerCount = message.GetUShort();
      lobbyVo.maxPlayerCount = message.GetUShort();
      lobbyVo.clients = new List<ClientVo>();
      for (int i = 0; i < lobbyVo.playerCount; i++)
      {
        ClientVo clientVo = new ClientVo();
        clientVo.id = message.GetUShort();
        clientVo.inLobbyId = message.GetUShort();
        //clientVo.userName = message.GetString();
        clientVo.colorId = message.GetUShort();
        lobbyVo.clients.Add(clientVo);
      }
      
      lobbyModel.lobbyVo = lobbyVo;

      dispatcher.Dispatch(LobbyEvent.ToLobbyManagerPanel);

    }
  }
}