using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Network.Command
{
  public class SendMessageCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.test);
      message.AddString("no prob"); //TODO: Samet: bu string'e bir bak.
      networkManager.Client.Send(message);

      DebugX.Log(DebugKey.Server, "Message sent");
    }
  }
}