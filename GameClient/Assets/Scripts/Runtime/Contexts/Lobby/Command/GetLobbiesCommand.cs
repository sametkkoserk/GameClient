using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
  public class GetLobbiesCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GetLobbies);
      message.AddSByte(0);
      networkManager.Client.Send(message);
      
      DebugX.Log(DebugKey.Request,"Get Lobbies message Sent");
    }
  }
}