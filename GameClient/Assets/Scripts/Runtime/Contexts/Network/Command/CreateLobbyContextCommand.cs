using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Network.Command
{
  public class CreateLobbyContextCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      Addressables.LoadSceneAsync(SceneKeys.LobbyScene, LoadSceneMode.Additive);
    }
  }
}