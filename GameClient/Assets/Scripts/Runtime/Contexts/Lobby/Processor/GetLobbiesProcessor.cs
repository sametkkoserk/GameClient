using System.Collections.Generic;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Processor
{
  public class GetLobbiesProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      var vo = (MessageReceivedVo)evt.data;
      var message = vo.message;
      var lobbies = networkManager.GetData<Dictionary<ushort, LobbyVo>>(message);

      // int lobbyCount = message.GetInt();
      // lobbiesVo.lobbies = new List<LobbyVo>();
      // for (int i = 0; i < lobbyCount; i++)
      // {
      //   LobbyVo lobbyVo = new LobbyVo();
      //   lobbyVo.lobbyId = message.GetUShort();
      //   lobbyVo.lobbyName = message.GetString();
      //   lobbyVo.isPrivate = message.GetBool();
      //   lobbyVo.playerCount = message.GetUShort();
      //   lobbyVo.maxPlayerCount = message.GetUShort();
      //   lobbyVo.leaderId = message.GetUShort();
      //   lobbiesVo.lobbies.Add(lobbyVo);
      // }
      Debug.Log("GetLobbies received");
      dispatcher.Dispatch(LobbyEvent.listLobbies, lobbies);
    }
  }
}