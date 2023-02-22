using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Command
{
  public class GetLobbiesCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GetLobbies);
      networkManager.Client.Send(message);
      Debug.Log("GetLobbies sent");
    }
  }
}