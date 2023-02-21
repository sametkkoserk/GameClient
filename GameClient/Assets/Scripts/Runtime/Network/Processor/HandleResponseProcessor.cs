using Riptide;
using Runtime.Network.Enum;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Network.Processor
{
    public class HandleResponseProcessor : EventCommand
    {
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            Message message = vo.message;
            
            dispatcher.Dispatch(NetworkEvent.CreateLobby);
        }

    }
}