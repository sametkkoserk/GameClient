using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
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
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Dictionary<string, LobbyVo> lobbies = networkManager.GetData<Dictionary<string, LobbyVo>>(vo.message);

      dispatcher.Dispatch(LobbyEvent.listLobbies, lobbies);
      
      DebugX.Log(DebugKey.Response,"Get Lobbies message Received");
    }
  }
}