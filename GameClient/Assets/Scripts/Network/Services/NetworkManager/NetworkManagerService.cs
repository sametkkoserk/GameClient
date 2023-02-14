using System;
using Network.Enum;
using Network.Vo;
using Riptide;
using Riptide.Utils;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Network.Services.NetworkManager
{
    public class NetworkManagerService :  INetworkManagerService
    {
        public Client Client { get; private set; }
        
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher{ get; set;}

        [SerializeField] private string ip;
        [SerializeField] private ushort port;

        public void Connect(string _ip, ushort _port)
        {
            ip = _ip;
            port = _port;
            
            RiptideLogger.Initialize(Debug.Log,Debug.Log,Debug.LogWarning,Debug.LogError,false);
            Client = new Client();
            
            Client.Connected += DidConnect;
            Client.ConnectionFailed += FailedToConnect;
            Client.Disconnected += DidDisconnect;
            
            Client.Connect($"{ip}:{port}");
            
            Client.MessageReceived+= MessageHandler;

            
        }

        public void Ticker()
        {
            if (Client != null)
            {
                Client.Update();
            }
        }
        
        public void MessageHandler(object sender, MessageReceivedEventArgs messageArgs)
        {
            MessageReceivedVo vo = new MessageReceivedVo();
            vo.message = messageArgs.Message;
            dispatcher.Dispatch((ServerToClientId)messageArgs.MessageId,vo);
        }

        private void DidConnect(object sender, EventArgs e)
        {
            Debug.Log("Connected");
            dispatcher.Dispatch(NetworkEvent.SendMessage);
        }

        private void FailedToConnect(object sender, EventArgs e)
        {
            Debug.Log("Connection Failed");
        }

        private void DidDisconnect(object sender, EventArgs e)
        {
            Debug.Log("Disconnected");
        }
        
        public void OnQuit()
        {
            Client.Disconnect();
        }
    }
}
