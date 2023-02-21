using Riptide;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Command
{
  public class OutFromLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.OutFromLobby);
      message.AddUShort(lobbyModel.lobbyVo.lobbyId);
      message.AddUShort(lobbyModel.inLobbyId);
      networkManager.Client.Send(message);
    }
  }
}