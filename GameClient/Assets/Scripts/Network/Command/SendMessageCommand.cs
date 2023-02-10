using Multiplayer.Enum;
using Multiplayer.Services.NetworkManager;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Multiplayer.Command
{
    public class SendMessageCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.test);
            message.AddString("no prob");
            networkManager.Client.Send(message);
            Debug.Log("Message sent");
            
            
        }
    }
}