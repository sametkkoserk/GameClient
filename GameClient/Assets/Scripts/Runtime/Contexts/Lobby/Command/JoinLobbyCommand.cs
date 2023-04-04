using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Command
{
  public class JoinLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      ushort lobbyId = (ushort)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.JoinLobby);
      message = networkManager.SetData(message, lobbyId);
      networkManager.Client.Send(message);
      Debug.Log("Join Lobby Sent");
    }
  }
}