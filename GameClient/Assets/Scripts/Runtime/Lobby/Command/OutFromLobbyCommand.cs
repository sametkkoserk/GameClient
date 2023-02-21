using Riptide;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Vo;
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
      OutFromLobbyVo outFromLobbyVo = new OutFromLobbyVo()
      {
        lobbyId = lobbyModel.lobbyVo.lobbyId,
        inLobbyId = lobbyModel.inLobbyId,
      };
      message=networkManager.SetData(message,outFromLobbyVo);
      
      networkManager.Client.Send(message);
    }
  }
}