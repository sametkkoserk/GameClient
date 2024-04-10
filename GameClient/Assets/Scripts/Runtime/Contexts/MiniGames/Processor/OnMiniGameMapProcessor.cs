using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class OnMiniGameMapProcessor : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;

            MiniGameMapGenerationVo vo = networkManager.GetData<MiniGameMapGenerationVo>(messageReceivedVo.message);
            dispatcher.Dispatch(MiniGamesEvent.MapReceived,vo);

            DebugX.Log(DebugKey.Response, $"Received: OnMiniGameMapProcessor");
        }

    }
}