using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.RootManager;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class OnMiniGameEndedProcessor : EventCommand
    {
        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossDispatcher { get; set; }
        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;

            crossDispatcher.Dispatch(MainGameEvent.CloseSceneRoot, RootKey.MiniGame);

            SceneManager.UnloadSceneAsync("MiniGames");
            DebugX.Log(DebugKey.Server, "Received: OnMiniGameEndedProcessor");
        }
    }
}