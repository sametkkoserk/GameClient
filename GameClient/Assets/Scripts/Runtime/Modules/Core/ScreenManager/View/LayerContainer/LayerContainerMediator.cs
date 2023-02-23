using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.View.PanelContainer;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.ScreenManager.View.LayerContainer
{
  public class LayerContainerMediator : EventMediator
  {
    [Inject]
    public LayerContainerView view { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
    }

    private void Start()
    {
      screenManagerModel.SetSortOrder();
      
      Debug.Log(gameObject.scene.name);
      
      for (int i = 0; i < view.layerVos.Count; i++)
      {
        if (!view.layerVos[i].active) continue;
        if (view.processedKeys.Contains(view.layerVos[i].key.ToString())) continue;

        view.processedKeys.Add(view.layerVos[i].key.ToString());
        
        GameObject instantiated = Instantiate(view.panelContainer, transform);
        PanelContainerView behaviour = instantiated.GetComponent<PanelContainerView>();

        behaviour.Init(view.layerVos[i].key);
      }
    }

    public override void OnRemove()
    {
    }
  }
}