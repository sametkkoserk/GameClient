using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class OnMiniGameEndedProcessor : EventCommand
    {
        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;

            SceneManager.UnloadSceneAsync("MiniGames");
            DebugX.Log(DebugKey.Server, "Received: OnMiniGameEndedProcessor");
        }
    }
}