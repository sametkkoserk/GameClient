using Main.Enum;
using Multiplayer.Enum;
using Multiplayer.Services.NetworkManager;
using RiptideNetworking;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Multiplayer.Command
{
    public class LoadNetworkSceneCommand : EventCommand
    {
        public override void Execute()
        {
            Addressables.LoadSceneAsync(SceneKeys.NetworkScene);
        }
    }
}