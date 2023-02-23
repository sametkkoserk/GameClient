using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.ScreenManager.View.PanelContainer
{
  public enum PanelContainerEvent
  {
    SetInitialData
  }
  public class PanelContainerMediator : EventMediator
  {
    [Inject]
    public PanelContainerView view { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(PanelContainerEvent.SetInitialData, SetInitialData);
    }

    private void SetInitialData()
    {
      view.canvas.sortingOrder = screenManagerModel.layerMap[view.key.ToString()];

      gameObject.transform.name = view.key.ToString();
    }

    private void Start()
    {
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PanelContainerEvent.SetInitialData, SetInitialData);
    }
  }
}