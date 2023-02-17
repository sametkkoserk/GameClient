using Lobby.View.CreateLobbyPanel;
using Lobby.Vo;
using Network.Enum;
using Network.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Lobby.Command
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
            message.AddUShort((ushort)6);
            networkManager.Client.Send(message);
            Debug.Log("CreateLobby Message sent");   
        }
    }
}