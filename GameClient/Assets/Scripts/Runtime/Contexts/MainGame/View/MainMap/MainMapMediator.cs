using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.View.City;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.MapGenerator, OnMapGenerator);
    }

    private void Start()
    {
      OnMapGenerator();
    }

    private void OnMapGenerator()
    {
      for (int i = 0; i < mainGameModel.cities.Count; i++)
      {
        int count = i;

        AsyncOperationHandle<GameObject> instantiateAsync = Addressables.InstantiateAsync(MainGameKeys.City, transform);

        instantiateAsync.Completed += handle =>
        {
          GameObject cityObject = instantiateAsync.Result;

          CityView cityView = cityObject.transform.GetComponent<CityView>();

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