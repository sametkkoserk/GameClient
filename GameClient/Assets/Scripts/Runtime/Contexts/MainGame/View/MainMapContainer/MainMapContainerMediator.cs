using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.MainGame.View.MainMapContainer
{
  public class MainMapContainerMediator : EventMediator
  {
    [Inject]
    public MainMapContainerView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.CreateMap, OnCreateMap);
    }

    public void OnCreateMap()
    {
      Addressables.InstantiateAsync(LobbyKey.MainMap, gameObject.transform);

      if (mainGameModel.materials.Count == 0)
      {
        // Player Vo olusturulacak.
        mainGameModel.materials = view.playerMaterials;
      }
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}