using System;
using Editor.Tools.DebugX.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.UnityConverters.Math;
using Riptide;
using Riptide.Utils;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Network.Services.NetworkManager
{
    public class NetworkManagerService :  INetworkManagerService
    {
        public Client Client { get; private set; }
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossDispatcher{ get; set;}
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher{ get; set;}

        private JsonSerializerSettings settings = new()
        {
            Converters = new JsonConverter[] {
                new Vector3Converter(),
                new StringEnumConverter(),
            },
            ContractResolver = new DefaultContractResolver(),
        };

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
                message = messageArgs.Message.GetString()
            };
            crossDispatcher.Dispatch((ServerToClientId)messageArgs.MessageId,vo);
        }

        public T GetData<T>(string message) where T : new()
        {
            return message== null ? default(T) : JsonConvert.DeserializeObject<T>(message);
        }
        public Message SetData(Message message, object obj)
        {
            if (obj == null)
                Debug.LogError("Set data object is null");
            string objStr = JsonConvert.SerializeObject(obj, settings);

            message.AddString(objStr);
            return message;
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
