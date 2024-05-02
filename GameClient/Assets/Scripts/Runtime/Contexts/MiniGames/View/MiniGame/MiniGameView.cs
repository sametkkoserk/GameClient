using System;
using System.Collections.Generic;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MiniGames.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Contexts.MiniGames.View.MiniGame
{
    public class MiniGameView : EventView
    {
        [Inject] 
        public ILobbyModel lobbyModel{ get; set; }
        public Transform playerContainer;

        public bool lastFrameSent;

        public ClickedButtonsVo clickedButtonsVo = new ClickedButtonsVo();
        public PlayerActions playerActions;

        public MapGenerator mapGenerator;

        private bool isButtonsChanged = false;

        protected override void Start()
        {
            base.Start();
            clickedButtonsVo.clickedButtons = new List<string>();
            clickedButtonsVo.releasedButtons = new List<string>();

            clickedButtonsVo.lobbyCode = lobbyModel.lobbyVo.lobbyCode;

            SetInputActionListeners();
        }

        private void SetInputActionListeners()
        {
            playerActions = new PlayerActions();
            playerActions.RacePlayerActionMap.Enable();
            
            playerActions.RacePlayerActionMap.vertical.started += OnAnyButtonClicked;
            playerActions.RacePlayerActionMap.horizontal.started += OnAnyButtonClicked;
            playerActions.RacePlayerActionMap.brake.started += OnAnyButtonClicked;
            
            playerActions.RacePlayerActionMap.vertical.canceled += OnAnyButtonReleased;
            playerActions.RacePlayerActionMap.horizontal.canceled += OnAnyButtonReleased;
            playerActions.RacePlayerActionMap.brake.canceled += OnAnyButtonReleased;
        }



        private void OnAnyButtonClicked(InputAction.CallbackContext obj)
        {
            
            Debug.Log("PlayerActionGot: "+obj.action.name+obj.ReadValue<float>());
            if (obj.action.name=="horizontal")
            {
                clickedButtonsVo.horizontalAxis = obj.ReadValue<float>();
            }
            else if (obj.action.name=="vertical")
            {
                clickedButtonsVo.verticalAxis = obj.ReadValue<float>();
            }
            else
            {
                clickedButtonsVo.clickedButtons.Add(obj.action.name);
            }

            isButtonsChanged = true;
        }
        private void OnAnyButtonReleased(InputAction.CallbackContext obj)
        {
            if (obj.action.name=="horizontal")
            {
                clickedButtonsVo.horizontalAxis = 0;
            }
            else if (obj.action.name=="vertical")
            {
                clickedButtonsVo.verticalAxis = 0;
            }
            else
            {
                clickedButtonsVo.releasedButtons.Add(obj.action.name);
            }
            isButtonsChanged = true;
        }
        void FixedUpdate()
        {
            if (playerActions.RacePlayerActionMap.horizontal.IsPressed())
            {
                float newValue = playerActions.RacePlayerActionMap.horizontal.ReadValue<float>();
                if (Math.Abs(newValue - clickedButtonsVo.horizontalAxis) > 0.1)
                {
                    isButtonsChanged = true;
                }
                clickedButtonsVo.horizontalAxis = newValue;
            }
            if (playerActions.RacePlayerActionMap.vertical.IsPressed())
            {
                float newValue = playerActions.RacePlayerActionMap.vertical.ReadValue<float>();
                if (Math.Abs(newValue - clickedButtonsVo.verticalAxis) > 0.1)
                {
                    isButtonsChanged = true;
                }
                clickedButtonsVo.verticalAxis = newValue;
            }
        }
        private void LateUpdate()
        {
            if (!isButtonsChanged)return;

            dispatcher.Dispatch(MiniGameEvent.ButtonClicked,clickedButtonsVo);
            ResetButtonsVo();
        }

        private void ResetButtonsVo()
        {
            clickedButtonsVo.clickedButtons.Clear();
            clickedButtonsVo.releasedButtons.Clear();
        }
    }
}