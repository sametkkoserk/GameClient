using Runtime.Main.Enum;
using Runtime.Network.Services.NetworkManager;
using strange.extensions.command.impl;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Network.Command
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