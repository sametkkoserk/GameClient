using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.Lobby.View.CreateLobbyPanel
{
  public enum CreateLobbyPanelEvent
  {
    Create,
    Back
  }

  public class CreateLobbyPanelMediator : EventMediator
  {
    [Inject]
    public CreateLobbyPanelView view { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(CreateLobbyPanelEvent.Create, OnCreate);
      view.dispatcher.AddListener(CreateLobbyPanelEvent.Back, OnBack);
    }

    public void OnCreate()
    {
      LobbyVo vo = new()
      {
        lobbyName = view.LobbyNameInputField.text,
        isPrivate = view.isPrivate.isOn,
        maxPlayerCount = 10
      };
      dispatcher.Dispatch(LobbyEvent.SendCreateLobby, vo);
    }

    private void OnBack()
    {
      screenManagerModel.OpenPanel(SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel, LobbyKey.LobbyPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Create, OnCreate);
      view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Back, OnBack);
    }
  }
}