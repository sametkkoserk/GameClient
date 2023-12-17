using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class ConfirmFortifyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      FortifyVo fortifyVo = (FortifyVo)evt.data;
      
      SendPacketWithLobbyCode<FortifyVo> vo = new()
      {
        mainClass = fortifyVo,
        lobbyCode = lobbyModel.lobbyVo.lobbyCode
      };
      
      // TODO: Safak bunlari tek satirda yapmayi dene.
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.Fortify);
      message = networkManager.SetData(message, vo);
      
      networkManager.Client.Send(message);
    }
  }
}