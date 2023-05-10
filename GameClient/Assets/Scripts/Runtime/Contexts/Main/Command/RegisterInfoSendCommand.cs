using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Main.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class RegisterInfoSendCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      PlayerRegisterInfoVo PlayerRegisterInfoVo = (PlayerRegisterInfoVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.Register);
      message = networkManager.SetData(message, PlayerRegisterInfoVo);
      networkManager.Client.Send(message);
      
      DebugX.Log(DebugKey.Request,"User register info message Sent");
    }
  }
}