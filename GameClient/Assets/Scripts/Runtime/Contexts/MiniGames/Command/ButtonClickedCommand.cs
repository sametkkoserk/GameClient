using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Command
{
    public class ButtonClickedCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            ClickedButtonsVo vo = (ClickedButtonsVo)evt.data;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.ButtonClicked);
            message = networkManager.SetData(message, vo);
            
            networkManager.Client.Send(message);
            DebugX.Log(DebugKey.Server,"Send:ClickedButtons");
        }

    }
}