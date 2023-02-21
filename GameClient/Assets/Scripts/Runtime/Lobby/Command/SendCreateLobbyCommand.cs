using Editor.Tools.DebugX.Runtime;
using Newtonsoft.Json;
using Riptide;
using Runtime.Lobby.Vo;
using Runtime.Network.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Command
{
    public class SendCreateLobbyCommand : EventCommand
    {
        [Inject] 
        public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            LobbyVo vo = (LobbyVo)evt.data;
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.createLobby);
            message=networkManager.SetData(message,vo);

            // message.AddString(vo.lobbyName);
            // message.AddBool(vo.isPrivate);
            // message.AddUShort(6);
            networkManager.Client.Send(message);
            
            DebugX.Log(DebugKey.Server,"Create Lobby Message sent");   
        }
    }
}