using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
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
      Addressables.InstantiateAsync(MainGameKeys.MainMap, gameObject.transform);
      
      
      // if (mainGameModel.materials.Count == 0)
      //   mainGameModel.materials = view.playerMaterials;
      // Player Vo olusturulacak.
      
      
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CreateMap, OnCreateMap);
    }
  }
}