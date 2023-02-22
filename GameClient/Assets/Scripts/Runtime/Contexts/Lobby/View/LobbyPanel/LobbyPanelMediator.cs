using Runtime.Contexts.Lobby.Enum;
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

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyPanelEvent.ToCreate,OnToCreate);
      view.dispatcher.AddListener(LobbyPanelEvent.ToJoin,OnToJoin);
    }

    private void OnToCreate()
    {
      dispatcher.Dispatch(LobbyEvent.ToCreatePanel);
    }
    private void OnToJoin()
    {
      dispatcher.Dispatch(LobbyEvent.ToJoinPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyPanelEvent.ToCreate,OnToCreate);
      view.dispatcher.RemoveListener(LobbyPanelEvent.ToJoin,OnToJoin);
    }
  }
}