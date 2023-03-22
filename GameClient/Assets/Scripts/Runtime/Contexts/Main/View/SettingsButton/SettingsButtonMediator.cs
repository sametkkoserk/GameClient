using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.SettingsButton
{
  public enum SettingsButtonEvent
  {
    OpenSettingsPanel
  }
  public class SettingsButtonMediator : EventMediator
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    [Inject]
    public SettingsButtonView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(SettingsButtonEvent.OpenSettingsPanel, OnOpenSettingsPanel);
    }

    private void OnOpenSettingsPanel()
    {
      crossDispatcher.Dispatch(MainEvent.OpenSettingsPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(SettingsButtonEvent.OpenSettingsPanel, OnOpenSettingsPanel);
    }
  }
}