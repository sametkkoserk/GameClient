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
      // dispatcher.Dispatch(LobbyEvent.ToCreatePanel);
      screenManagerModel.OpenPanel(SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel, LobbyKey.CreateLobbyPanel);
    }

    private void OnToJoin()
    {
      // dispatcher.Dispatch(LobbyEvent.ToJoinPanel);
      screenManagerModel.OpenPanel(SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel, LobbyKey.JoinLobbyPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyPanelEvent.ToCreate, OnToCreate);
      view.dispatcher.RemoveListener(LobbyPanelEvent.ToJoin, OnToJoin);
    }
  }
}