using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.RootManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class OnMiniGameCreateProcessor : EventCommand
    {
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossDispatcher { get; set; }
        [Inject] public INetworkManagerService networkManager { get; set; }

        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;
            
            crossDispatcher.Dispatch(MainGameEvent.CloseSceneRoot, RootKey.MainGame);

            MiniGameCreatedVo vo = networkManager.GetData<MiniGameCreatedVo>(messageReceivedVo.message);
            dispatcher.Dispatch(MiniGamesEvent.OnCreateMiniGame,vo);

            DebugX.Log(DebugKey.Response, $"Received: OnMiniGameCreateProcessor");
        }
    }
}