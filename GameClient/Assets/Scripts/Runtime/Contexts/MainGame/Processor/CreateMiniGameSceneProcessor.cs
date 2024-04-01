using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.MainGame.Processor
{
    public class CreateMiniGameSceneProcessor : EventCommand
    {
        [Inject]
        public IMainGameModel mainGameModel { get; set; }
        public override void Execute()
        {
            dispatcher.Dispatch(MainGameEvent.ToTheMiniGame);
            Addressables.LoadSceneAsync(SceneKeys.MiniGamesScene, LoadSceneMode.Additive);
        }
    }
}


