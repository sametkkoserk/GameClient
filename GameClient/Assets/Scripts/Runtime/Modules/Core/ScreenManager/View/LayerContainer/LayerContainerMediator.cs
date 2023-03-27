using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.View.PanelContainer;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.ScreenManager.View.LayerContainer
{
  public class LayerContainerMediator : EventMediator
  {
    [Inject]
    public LayerContainerView view { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    private void Start()
    {
      for (var i = 0; i < view.layerVos.Count; i++)
      {
        if (!view.layerVos[i].active) continue;
        if (view.processedKeys.Contains(view.layerVos[i].key.ToString())) continue;

        view.processedKeys.Add(view.layerVos[i].key.ToString());

        var instantiated = Instantiate(view.panelContainer, transform);
        var behaviour = instantiated.GetComponent<PanelContainerView>();

        behaviour.Init(view.layerVos[i].key);
      }

      dispatcher.Dispatch(PanelEvent.PanelContainersCreated);
    }

    public override void OnRegister()
    {
      Init();
    }

    private void Init()
    {
      screenManagerModel.AddLayerContainer(transform.gameObject.scene.name);

      screenManagerModel.SetSortOrder();
    }

    public override void OnRemove()
    {
    }
  }
}