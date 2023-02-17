using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Lobby.Command
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