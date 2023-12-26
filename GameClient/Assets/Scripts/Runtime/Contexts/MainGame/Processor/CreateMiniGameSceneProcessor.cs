using Runtime.Contexts.Main.Enum;
using StrangeIoC.scripts.strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.MainGame.Processor
{
    public class CreateMiniGameSceneProcessor : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.MiniGamesScene, LoadSceneMode.Additive);

        }
    }
}


