using Lobby.Model.LobbyModel;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Command
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