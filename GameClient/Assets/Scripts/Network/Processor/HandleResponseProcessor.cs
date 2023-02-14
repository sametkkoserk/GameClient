using Network.Enum;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace Network.Processor
{
    public class HandleResponseProcessor : EventCommand
    {
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo)evt.data;
            Message message = vo.message;
            string testMessage = message.GetString();
            
            Debug.Log(testMessage);
        }

    }
}