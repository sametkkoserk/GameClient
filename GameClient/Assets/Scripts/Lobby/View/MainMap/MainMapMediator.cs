using Lobby.Model.LobbyModel;
using strange.extensions.mediation.impl;

namespace Lobby.View.MainMap
{
  public enum MainMapEvent
  {
  }

  public class MainMapMediator : EventMediator
  {
    [Inject]
    public MainMapView view { get; set; }


    public override void OnRegister()
    {
    }


    public override void OnRemove()
    {
    }
  }
}