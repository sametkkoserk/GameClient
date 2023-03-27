using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Command
{
  public class PlayerReadyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      var message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.PlayerReady);
      PlayerReadyVo playerReadyVo = new()
      {
        lobbyId = lobbyModel.lobbyVo.lobbyId,
        inLobbyId = lobbyModel.clientVo.inLobbyId
      };
      message = networkManager.SetData(message, playerReadyVo);

      networkManager.Client.Send(message);

      Debug.Log("Player is ready!");
    }
  }
}