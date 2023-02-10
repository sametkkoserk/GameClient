using Multiplayer.Enum;
using Riptide;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.impl;
using UnityEngine;

namespace Multiplayer.Processor
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