using Lobby.Enum;
using Lobby.Model.LobbyModel;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Lobby.View.MainMapContainer
{
  public class MainMapContainerMediator : EventMediator
  {
    [Inject]
    public MainMapContainerView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(LobbyEvent.StartGame, CreateMap);
    }

    public void CreateMap()
    {
      Addressables.InstantiateAsync(LobbyKey.MainMap, gameObject.transform);

      lobbyModel.materials = view.playerMaterials;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LobbyEvent.StartGame, CreateMap);
    }
  }
}