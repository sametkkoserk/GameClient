using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.CreateLobbyPanel
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