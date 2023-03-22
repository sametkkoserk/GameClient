using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Network.Command
{
  public class SendMessageCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      // Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.test);
      // message.AddString("no prob"); //TODO: Samet: bu string'e bir bak.
      // networkManager.Client.Send(message);
      //
      // DebugX.Log(DebugKey.Server, "Message sent");
    }
  }
}