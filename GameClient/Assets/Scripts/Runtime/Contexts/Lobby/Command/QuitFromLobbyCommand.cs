using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class QuitFromLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.QuitFromLobby);
      
      QuitFromLobbyVo quitFromLobbyVo = new()
      {
        lobbyCode = lobbyModel.lobbyVo.lobbyCode,
        id = lobbyModel.clientVo.id
      };
      
      message = networkManager.SetData(message, quitFromLobbyVo);
      networkManager.Client.Send(message);
      
      DebugX.Log(DebugKey.Request,"Quit From Lobby message Sent");

    }
  }
}