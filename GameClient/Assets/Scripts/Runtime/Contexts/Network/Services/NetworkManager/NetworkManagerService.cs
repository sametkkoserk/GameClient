using System;
using System.IO;
using Editor.Tools.DebugX.Runtime;
using ProtoBuf;
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
  public class NetworkManagerService : INetworkManagerService
  {
    private string ip;
    private ushort port;

    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }

    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public Client Client { get; private set; }

    public void Connect(string _ip, ushort _port)
    {
      ip = _ip;
      port = _port;

      RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
      Client = new Client();

      Client.Connected += DidConnect;
      Client.ConnectionFailed += FailedToConnect;
      Client.Disconnected += DidDisconnect;

      Client.Connect($"{ip}:{port}");

      Client.MessageReceived += MessageHandler;
    }

    public void Ticker()
    {
      Client?.Update();
    }

    public T GetData<T>(byte[] message) where T : new()
    {
      using MemoryStream stream = new(message);
      return message == null ? default : Serializer.Deserialize<T>(stream);
    }

    public Message SetData(Message message, object obj)
    {
      if (obj == null)
        Debug.LogError("Set data object is null");
      byte[] objBytes = ProtoSerialize<object>(obj);

      message.AddBytes(objBytes);
      return message;
    }

    private byte[] ProtoSerialize<T>(T message) where T : new()
    {
      using MemoryStream stream = new MemoryStream();
      Serializer.Serialize(stream, message);
      return stream.ToArray();
    }


    public void OnQuit()
    {
      Client.Disconnect();
    }

    public void MessageHandler(object sender, MessageReceivedEventArgs messageArgs)
    {
      MessageReceivedVo vo = new()
      {
        message = messageArgs.Message.GetBytes()
      };
      crossDispatcher.Dispatch((ServerToClientId)messageArgs.MessageId, vo);
    }

    private void DidConnect(object sender, EventArgs e)
    {
      DebugX.Log(DebugKey.Server, "Connected");
      dispatcher.Dispatch(NetworkEvent.SendMessage);
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
      DebugX.Log(DebugKey.Server, "Connection Failed");
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
      DebugX.Log(DebugKey.Server, "Disconnected");
    }


  }
}