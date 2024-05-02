using Runtime.Contexts.MainGame.Enum;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.RootManager
{
  public class RootManagerMediator : EventMediator
  {
    [Inject]
    public RootManagerView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.CloseSceneRoot, OnCloseRoot);
    }

    private void OnCloseRoot(IEvent payload)
    {
      RootKey rootKey = (RootKey)payload.data;

      print(gameObject.scene.name);
      if (rootKey.ToString() != gameObject.scene.name)
      {
        gameObject.SetActive(true);
        return;
      }
      
      gameObject.SetActive(false);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.CloseSceneRoot, OnCloseRoot);
    }
  }
}