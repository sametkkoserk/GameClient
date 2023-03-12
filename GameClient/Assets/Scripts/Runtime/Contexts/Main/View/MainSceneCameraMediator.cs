using Runtime.Contexts.Main.Enum;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Main.View
{
  public class MainSceneCameraMediator : EventMediator
  {
    [Inject]
    public MainSceneCameraView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainEvent.OpenMainSceneCamera, OnOpenCamera);
      dispatcher.AddListener(MainEvent.CloseMainSceneCamera, OnCloseCamera);
    }

    private void OnCloseCamera()
    {
      gameObject.SetActive(false);
      Debug.Log("de");
    }

    private void OnOpenCamera()
    {
      gameObject.SetActive(true);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainEvent.OpenMainSceneCamera, OnOpenCamera);
      dispatcher.RemoveListener(MainEvent.OpenMainSceneCamera, OnCloseCamera);
    }
  }
}