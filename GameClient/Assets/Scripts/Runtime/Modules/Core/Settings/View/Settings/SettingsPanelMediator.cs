using System.Collections.Generic;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine.SceneManagement;

namespace Runtime.Modules.Core.Settings.View.Settings
{
  public enum SettingsPanelEvent
  {
    ClosePanel,
    Logout
  }

  public class SettingsPanelMediator : EventMediator
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    [Inject]
    public SettingsPanelView view { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(SettingsPanelEvent.ClosePanel, OnClosePanel);
      view.dispatcher.AddListener(SettingsPanelEvent.Logout, OnLogout);

    }

    private void OnLogout()
    {
      List<string> scenesToRemove = new List<string>() { "MainGame", "Lobby", "MiniGame" };
      for (int i = 0; i < SceneManager.sceneCount; i++)
      {
        Scene scene = SceneManager.GetSceneAt(i);
        if (scenesToRemove.Contains(scene.name))
        {
          SceneManager.UnloadSceneAsync(scene.name);
        }
      }
      
      screenManagerModel.OpenPanel(MainPanelKey.RegisterPanel, SceneKey.Main, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
      OnClosePanel();
    }

    private void OnClosePanel()
    {
      screenManagerModel.CloseLayerPanels(LayerKey.SettingsLayer);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(SettingsPanelEvent.ClosePanel, OnClosePanel);
      view.dispatcher.RemoveListener(SettingsPanelEvent.Logout, OnLogout);

    }
  }
}