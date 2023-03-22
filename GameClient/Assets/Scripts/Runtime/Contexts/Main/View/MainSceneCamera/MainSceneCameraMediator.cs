using Runtime.Contexts.Main.Enum;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Main.View.MainSceneCamera
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