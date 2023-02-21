using Riptide;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Lobby.Command
{
  public class PlayerReadyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.PlayerReady);
      message.AddUShort(lobbyModel.lobbyVo.lobbyId);
      message.AddUShort(lobbyModel.inLobbyId);
      networkManager.Client.Send(message);
      
      Debug.Log("Player is ready!");
    }
  }
}