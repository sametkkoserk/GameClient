using System.Collections.Generic;
using Lobby.Enum;
using Lobby.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Lobby.Processor
{
  public class GetLobbiesProcessor : EventCommand
  {

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Message message = vo.message;

      int lobbyCount = message.GetInt();
      LobbiesVo lobbiesVo = new LobbiesVo();
      lobbiesVo.lobbies = new List<LobbyVo>();
      for (int i = 0; i < lobbyCount; i++)
      {
        LobbyVo lobbyVo = new LobbyVo();
        lobbyVo.lobbyId = message.GetUShort();
        lobbyVo.lobbyName = message.GetString();
        lobbyVo.isPrivate = message.GetBool();
        lobbyVo.leaderId = message.GetUShort();
        lobbiesVo.lobbies.Add(lobbyVo);
      }
      Debug.Log("GetLobbies received");
      dispatcher.Dispatch(LobbyEvent.listLobbies,lobbiesVo);
    }
  }
}