using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.Settings.View.Settings
{
  public enum SettingsPanelEvent
  {
    ClosePanel
  }
  public class SettingsPanelMediator : EventMediator
  {
    [Inject]
    public SettingsPanelView view { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(SettingsPanelEvent.ClosePanel, OnClosePanel);
    }

    private void OnClosePanel()
    {
      screenManagerModel.CloseLayerPanels(LayerKey.SettingsLayer);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(SettingsPanelEvent.ClosePanel, OnClosePanel);
    }
  }
}