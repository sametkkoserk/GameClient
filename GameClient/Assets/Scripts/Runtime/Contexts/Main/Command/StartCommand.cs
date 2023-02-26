using Runtime.Contexts.Main.Enum;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Main.Command
{
    public class StartCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.NetworkScene, LoadSceneMode.Additive);
        }
    }
}