using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Command
{
    public class AddBotCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            string lobbyCode = (string)evt.data;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.AddBot);
            message = networkManager.SetData(message, lobbyCode);
            networkManager.Client.Send(message);
        }

    }
}