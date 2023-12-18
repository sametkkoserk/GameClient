using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class PassCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    public override void Execute()
    {
      SendPacketWithLobbyCode<int> vo = new()
      {
        mainClass = 1,
        lobbyCode = lobbyModel.lobbyVo.lobbyCode
      };
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.Pass);
      message = networkManager.SetData(message, vo);
      
      networkManager.Client.Send(message);
    }
  }
}