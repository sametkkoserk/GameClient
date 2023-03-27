using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.View.City;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.MainGame.View.MainMap
{
  public enum MainMapEvent
  {
  }

  public class MainMapMediator : EventMediator
  {
    [Inject]
    public MainMapView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    private void Start()
    {
      OnMapGenerator();
    }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.MapGenerator, OnMapGenerator);
    }

    private void OnMapGenerator()
    {
      for (var i = 0; i < mainGameModel.cities.Count; i++)
      {
        var count = i;

        var instantiateAsync = Addressables.InstantiateAsync(MainGameKeys.City, transform);

        instantiateAsync.Completed += handle =>
        {
          var cityObject = instantiateAsync.Result;

          var cityView = cityObject.transform.GetComponent<CityView>();

          cityView.Init(mainGameModel.cities.ElementAt(count).Value);
        };
      }
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.MapGenerator, OnMapGenerator);
    }
  }
}