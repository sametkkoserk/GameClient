using Network.Enum;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Network.Processor
{
    public class HandleResponseProcessor : EventCommand
    {
        [MessageHandler((ushort)ServerToClientId.response)]
        private static void Test(Message message)
        {
            Debug.Log(message.GetString());
        }
    }
}