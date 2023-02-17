using System;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.View.JoinLobbyPanel;
using Lobby.Vo;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = System.Object;

namespace Lobby.View.LobbyManagerPanel
{
  public enum LobbyManagerPanelEvent
  {
    Ready,
    Back
  }
  public class LobbyManagerPanelMediator : EventMediator
  {
    [Inject]
    public LobbyManagerPanelView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Ready,OnReady);
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Back,OnBack);
    }

    private void Start()
    {
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.lobbyNameText.text = lobbyVo.lobbyName;
      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;
      for (int i = 0; i < lobbyVo.playerCount; i++)
      {
        int count = i;
        var instantiateAsync = Addressables.InstantiateAsync(LobbyKey.LobbyManagerPanelItem, view.playerContainer);
        instantiateAsync.Completed += handle =>
        {
          ClientVo clientVo = lobbyVo.clients[count];
          GameObject obj = instantiateAsync.Result;
          LobbyManagerPanelItemBehaviour behaviour = obj.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
          behaviour.Init(clientVo,lobbyModel.colors[clientVo.inLobbyId]);
        };
      }
    }

    private void OnReady()
    {
      dispatcher.Dispatch(LobbyEvent.SendReady);
    }
    
    private void OnBack()
    {
      dispatcher.Dispatch(LobbyEvent.BackToLobbyPanel);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Ready,OnReady);
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Back,OnBack);
    }
  }
}