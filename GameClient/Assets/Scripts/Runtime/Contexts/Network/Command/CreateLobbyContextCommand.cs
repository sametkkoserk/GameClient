using Runtime.Contexts.Main.Enum;
using StrangeIoC.scripts.strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Network.Command
{
  public class CreateLobbyContextCommand : EventCommand
  {
    public override void Execute()
    {
      Addressables.LoadSceneAsync(SceneKeys.LobbyScene, LoadSceneMode.Additive);
    }
  }
}