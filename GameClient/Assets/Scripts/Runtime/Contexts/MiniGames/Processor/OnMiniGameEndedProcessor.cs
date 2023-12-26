using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.MiniGames.Processor
{
    public class OnMiniGameEndedProcessor : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }
        

        public override void Execute()
        {
            MessageReceivedVo messageReceivedVo = (MessageReceivedVo)evt.data;

            SceneManager.UnloadSceneAsync("MiniGames");
            DebugX.Log(DebugKey.Server, $"Received: OnMiniGameEndedProcessor");
        }

    }
}