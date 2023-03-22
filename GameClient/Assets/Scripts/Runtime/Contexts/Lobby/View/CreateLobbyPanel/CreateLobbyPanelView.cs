using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.CreateLobbyPanel
{
  public class CreateLobbyPanelView : EventView
  {
    public TMP_InputField LobbyNameInputField;

    public Toggle isPrivate;

    public void OnCreate()
    {
      dispatcher.Dispatch(CreateLobbyPanelEvent.Create);
    }

    public void OnBack()
    {
      dispatcher.Dispatch(CreateLobbyPanelEvent.Back);
    }
  }
}