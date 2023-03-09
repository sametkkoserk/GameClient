using Runtime.Contexts.Lobby.Enum;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.Lobby.View.LobbyPanel
{
  public enum LobbyPanelEvent
  {
    ToCreate,
    ToJoin
  }

  public class LobbyPanelMediator : EventMediator
  {
    [Inject]
    public LobbyPanelView view { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyPanelEvent.ToCreate, OnToCreate);
      view.dispatcher.AddListener(LobbyPanelEvent.ToJoin, OnToJoin);
    }

    private void OnToCreate()
    {
      screenManagerModel.OpenPanel(LobbyKey.CreateLobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    private void OnToJoin()
    {
      screenManagerModel.OpenPanel(LobbyKey.JoinLobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyPanelEvent.ToCreate, OnToCreate);
      view.dispatcher.RemoveListener(LobbyPanelEvent.ToJoin, OnToJoin);
    }
  }
}