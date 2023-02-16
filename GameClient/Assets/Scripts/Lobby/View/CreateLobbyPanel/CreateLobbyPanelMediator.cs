using Lobby.Enum;
using Lobby.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Lobby.View.CreateLobbyPanel
{
    public enum CreateLobbyPanelEvent
    {
        Create
    }
    public class CreateLobbyPanelMediator : EventMediator
    {
        [Inject] public CreateLobbyPanelView view { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(CreateLobbyPanelEvent.Create,OnCreate);
        }

        public void OnCreate()
        {
            LobbyVo vo = new LobbyVo();
            vo.lobbyName = view.LobbyNameInputField.text;
            vo.isPrivate = view.isPrivate.isOn;
            Debug.Log("Button Clicked");
            dispatcher.Dispatch(LobbyEvent.SendCreateLobby,vo);
        }
        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(CreateLobbyPanelEvent.Create,OnCreate);
        }
    }
}