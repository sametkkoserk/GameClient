using Main.Enum;
using Network.Services.NetworkManager;
using strange.extensions.command.impl;
using Unity.VisualScripting;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Network.Command
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