using System;
using Editor.Tools.DebugX.Runtime;
using Riptide;
using Riptide.Utils;
using Runtime.Network.Enum;
using Runtime.Network.Vo;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Runtime.Network.Services.NetworkManager
{
    public class NetworkManagerService :  INetworkManagerService
    {
        public Client Client { get; private set; }
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossDispatcher{ get; set;}
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher{ get; set;}

        private string ip;
        private ushort port;

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
            MessageReceivedVo vo = new()
            {
                message = messageArgs.Message
            };
            crossDispatcher.Dispatch((ServerToClientId)messageArgs.MessageId,vo);
        }

        private void DidConnect(object sender, EventArgs e)
        {
            DebugX.Log(DebugKey.Server,"Connected");
            dispatcher.Dispatch(NetworkEvent.SendMessage);
        }

        private void FailedToConnect(object sender, EventArgs e)
        {
            DebugX.Log(DebugKey.Server,"Connection Failed");
        }

        private void DidDisconnect(object sender, EventArgs e)
        {
            DebugX.Log(DebugKey.Server,"Disconnected");
        }
        
        public void OnQuit()
        {
            Client.Disconnect();
        }
    }
}
