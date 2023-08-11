using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Command
{
  public class ReadyToGameStartCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    
    // [Inject]
    // public IDiscordModel discordModel { get; set; }

    public override void Execute()
    {
      GameStartVo vo = new()
      {
        gameStart = true,
        lobbyCode = lobbyModel.lobbyVo.lobbyCode
      };
      
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GameStart);
      message = networkManager.SetData(message, vo);
      
      networkManager.Client.Send(message);
      
      // discordModel.InGame(playerModel.playerRegisterInfoVo.username);
    }
  }
}