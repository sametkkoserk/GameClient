using Editor.Tools.DebugX.Runtime;
using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
    public class SendCreateLobbyCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            LobbyVo vo = (LobbyVo)evt.data;
            
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.createLobby);
            message.AddString(vo.lobbyName);
            message.AddBool(vo.isPrivate);
            message.AddUShort(6);
            networkManager.Client.Send(message);
            
            DebugX.Log(DebugKey.Server,"Create Lobby Message sent");   
        }
    }
}