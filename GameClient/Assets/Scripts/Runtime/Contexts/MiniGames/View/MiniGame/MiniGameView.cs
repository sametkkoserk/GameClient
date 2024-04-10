using System.Collections.Generic;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MiniGames.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.View.MiniGame
{
    public class MiniGameView : EventView
    {
        [Inject] 
        public ILobbyModel lobbyModel{ get; set; }
        public Transform playerContainer;

        public bool lastFrameSent;

        public ClickedButtonsVo clickedButtonsVo = new ClickedButtonsVo();

        public MapGenerator mapGenerator;

        protected override void Start()
        {
            base.Start();
            clickedButtonsVo.buttons = new List<string>();
            clickedButtonsVo.lobbyCode = lobbyModel.lobbyVo.lobbyCode;
        }

        void FixedUpdate()
        {
            clickedButtonsVo.verticalAxis = Input.GetAxis("Vertical");
            clickedButtonsVo.horizontalAxis = Input.GetAxis("Horizontal");
            clickedButtonsVo.buttons.Clear();
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                
                if (Input.GetKey(keyCode))
                {
                    clickedButtonsVo.buttons.Add(keyCode.ToString());
                }
            }

            if (clickedButtonsVo.buttons.Count>0)
            {
                lastFrameSent = true;
                dispatcher.Dispatch(MiniGameEvent.ButtonClicked,clickedButtonsVo);
            }
            else if(lastFrameSent)
            {
                lastFrameSent = false;
            }
            
        }
    }


}