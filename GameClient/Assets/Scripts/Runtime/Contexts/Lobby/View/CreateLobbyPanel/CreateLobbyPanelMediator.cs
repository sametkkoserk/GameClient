using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

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

    ///<summary>Create a Lobby. LobbySettingsVo holds standard settings of lobbies.</summary>
    public void OnCreate()
    {
      LobbySettingsVo settingsVo = new()
      {
        turnTime = 60
      };
      
      LobbyVo lobbyVo = new()
      {
        lobbyName = view.LobbyNameInputField.text,
        isPrivate = view.isPrivate.isOn,
        maxPlayerCount = 10,
        lobbySettingsVo = settingsVo
      };
      
      dispatcher.Dispatch(LobbyEvent.SendCreateLobby, lobbyVo);
    }

    private void OnBack()
    {
      screenManagerModel.OpenPanel(LobbyKey.LobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Create, OnCreate);
      view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Back, OnBack);
    }
  }
}