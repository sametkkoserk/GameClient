using Riptide;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Network.Processor
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