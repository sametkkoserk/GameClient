using Riptide;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Lobby.Command
{
  public class JoinLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      ushort lobbyId = (ushort)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.JoinLobby);
      message.AddUShort(lobbyId);
      networkManager.Client.Send(message);
      Debug.Log("Join Lobby Sent");
    }
  }
}