using Riptide;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Vo;
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
      PlayerReadyVo playerReadyVo = new PlayerReadyVo()
      {
        lobbyId = lobbyModel.lobbyVo.lobbyId,
        inLobbyId = lobbyModel.inLobbyId
      };
      message=networkManager.SetData(message,playerReadyVo);

      networkManager.Client.Send(message);
      
      Debug.Log("Player is ready!");
    }
  }
}