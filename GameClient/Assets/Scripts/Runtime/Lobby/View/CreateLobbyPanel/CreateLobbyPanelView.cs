using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace Runtime.Lobby.View.CreateLobbyPanel
{
    public class CreateLobbyPanelView : EventView
    {
        public TMP_InputField LobbyNameInputField;
        public Toggle isPrivate;
        public Button createButton;

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