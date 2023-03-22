using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Lobby.View.LobbyPanel
{
  public class LobbyPanelView : EventView
  {
    public void OnCreate()
    {
      dispatcher.Dispatch(LobbyPanelEvent.ToCreate);
    }
    
    public void OnJoin()
    {
      dispatcher.Dispatch(LobbyPanelEvent.ToJoin);
    }
  }
}