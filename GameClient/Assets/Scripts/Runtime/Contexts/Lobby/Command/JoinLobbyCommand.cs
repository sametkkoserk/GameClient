using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class JoinLobbyCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      string lobbyCode = (string)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.JoinLobby);
      message = networkManager.SetData(message, lobbyCode);
      networkManager.Client.Send(message);
      
      DebugX.Log(DebugKey.Request,"Join Lobby message Sent");
    }
  }
}