using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Contexts.MiniGames.View.MiniGame
{    public enum MiniGameEvent
    {
        ButtonClicked,
    }
    public class MiniGameMediator : EventMediator
    {
        [Inject] public MiniGameView view { get; set; }

        [Inject] public ILobbyModel lobbyModel { get; set; }
        
        
        public Dictionary<string, GameObject> objs=new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> newObjs=new Dictionary<string, GameObject>();
        public Dictionary<ushort, GameObject> players=new Dictionary<ushort, GameObject>();
        private MiniGameStateVo stateVo;

        private void Start()
        {
            //throw new NotImplementedException();
        }


        public override void OnRegister()
        {
            base.OnRegister();
            view.dispatcher.AddListener(MiniGameEvent.ButtonClicked,OnButtonClick);

            dispatcher.AddListener(MiniGamesEvent.StateReceived,OnStateReceived);
        }

        private void OnButtonClick(IEvent payload)
        {
            ClickedButtonsVo vo = (ClickedButtonsVo)payload.data;
            Debug.Log("Step2");
            dispatcher.Dispatch(MiniGamesEvent.ButtonClicked,vo);
        }

        private void OnStateReceived(IEvent payload)
        {
            stateVo = (MiniGameStateVo)payload.data;
            SetNewObjs();
            SetObjs();
            SetPlayers();

        }

        private void SetPlayers()
        {
            Dictionary<ushort, Vector3Vo> poss = stateVo.playerPositions;
            Dictionary<ushort, QuaternionVo> rots = stateVo.playerRotations;
            if (poss==null)return;

            
            for (int i = 0; i < poss.Count; i++)
            {
                var posKvp = poss.ElementAt(i);
                if (players.ContainsKey(posKvp.Key))
                {
                    if (players[posKvp.Key]==null)continue;
                    players[posKvp.Key].transform.position = posKvp.Value.ToVector3();
                    players[posKvp.Key].transform.rotation = rots[posKvp.Key].ToQuaternion();
                }
                else
                {
                    players[posKvp.Key] = null;

                    Addressables.InstantiateAsync(lobbyModel.clientVo.id==posKvp.Key?"myPlayer":"player", view.playerContainer).Completed+= handle =>
                    {
                        if (handle.Status==AsyncOperationStatus.Succeeded)
                        {
                            Transform playerTransform = handle.Result.transform;
                            playerTransform.position = posKvp.Value.ToVector3();
                            playerTransform.rotation = rots[posKvp.Key].ToQuaternion();
                            players[posKvp.Key] = handle.Result;
                            
                        }
                    };
                }
            }

        }

        

        private void SetObjs()
        {
            //throw new NotImplementedException();
        }

        private void SetNewObjs()
        {
            //throw new NotImplementedException();
        }




        public override void OnRemove()
        {
            base.OnRemove();
            view.dispatcher.RemoveListener(MiniGameEvent.ButtonClicked,OnButtonClick);

            dispatcher.RemoveListener(MiniGamesEvent.StateReceived,OnStateReceived);

        }
    }
}