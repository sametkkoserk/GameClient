using Runtime.Contexts.Main.Model.PopupPanelModel;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.Popup
{
  public enum PopupPanelEvent
  {
    ClosePanel
  }
  public class PopupPanelMediator : EventMediator
  {
    [Inject]
    public PopupPanelView view { get; set; }
    
    [Inject]
    public IPopupPanelModel popupPanelModel { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(PopupPanelEvent.ClosePanel, OnClose);
      
      Init();
    }

    private void Init()
    {
      view.titleText.text = popupPanelModel.popupInfoVo.titleText;
      view.contentText.text = popupPanelModel.popupInfoVo.contentText;
    }
    
    private void OnClose()
    {
      screenManagerModel.CloseLayerPanels(LayerKey.PopupLayer);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(PopupPanelEvent.ClosePanel, OnClose);
    }
  }
}