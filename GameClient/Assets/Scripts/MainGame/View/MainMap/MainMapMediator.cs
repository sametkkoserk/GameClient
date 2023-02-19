using System.Collections.Generic;
using MainGame.Enum;
using MainGame.View.City;
using MainGame.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MainGame.View.MainMap
{
  public enum MainMapEvent
  {
  }

  public class MainMapMediator : EventMediator
  {
    [Inject]
    public MainMapView view { get; set; }


    public override void OnRegister()
    {
      Debug.Log("MainMapRegister");
      dispatcher.AddListener(MainGameEvent.MapGenerator, OnMapGenerator);
    }

    private void OnMapGenerator(IEvent payload)
    {
      Dictionary<int, CityVo> cityVos = (Dictionary<int, CityVo>)payload.data;

      for (int i = 0; i < cityVos.Count; i++)
      {
        int count = i;
        
        AsyncOperationHandle<GameObject> instantiateAsync = Addressables.InstantiateAsync(MainGameKeys.City, transform);
        
        instantiateAsync.Completed += handle =>
        {
          GameObject cityObject = instantiateAsync.Result;

          CityView cityView = cityObject.transform.GetComponent<CityView>();

          cityView.Init(cityVos[count]);
        };
      }
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.MapGenerator, OnMapGenerator);
    }
  }
}