using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Lobby.View.CreateLobbyPanel
{
    public enum CreateLobbyPanelEvent
    {
        Create,
        Back
    }
    public class CreateLobbyPanelMediator : EventMediator
    {
        [Inject] public CreateLobbyPanelView view { get; set; }

        public override void OnRegister()
        { 
            view.dispatcher.AddListener(CreateLobbyPanelEvent.Create,OnCreate);
            view.dispatcher.AddListener(CreateLobbyPanelEvent.Back,OnBack);
        }

        public void OnCreate()
        {
            LobbyVo vo = new()
            {
                lobbyName = view.LobbyNameInputField.text,
                isPrivate = view.isPrivate.isOn
            };
            dispatcher.Dispatch(LobbyEvent.SendCreateLobby,vo);
        }
        
        private void OnBack()
        {
            dispatcher.Dispatch(LobbyEvent.BackToLobbyPanel);
        }
        
        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Create,OnCreate);
            view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Back,OnBack);

        }
    }
}