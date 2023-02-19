using System;
using System.Collections.Generic;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.View.JoinLobbyPanel;
using Lobby.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
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
      dispatcher.AddListener(LobbyEvent.NewPlayerToLobby,OnNewPlayer);
      dispatcher.AddListener(LobbyEvent.PlayerReadyResponse,OnPlayerReadyResponse);
      dispatcher.AddListener(LobbyEvent.PlayerIsOut,OnPlayerIsOut);
      
    }

    
    private void Start()
    {
      view.behaviours = new Dictionary<ushort, LobbyManagerPanelItemBehaviour>();
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.lobbyNameText.text = lobbyVo.lobbyName;
      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;
      for (ushort i = 0; i < lobbyVo.playerCount; i++)
      {
        ushort count = i;
        var instantiateAsync = Addressables.InstantiateAsync(LobbyKey.LobbyManagerPanelItem, view.playerContainer);
        instantiateAsync.Completed += handle =>
        {
          ClientVo clientVo = lobbyVo.clients[count];
          GameObject obj = instantiateAsync.Result;
          LobbyManagerPanelItemBehaviour behaviour = obj.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
          behaviour.Init(clientVo,lobbyModel.colors[clientVo.inLobbyId]);
          
          view.behaviours[clientVo.inLobbyId]=behaviour;
        };
      }
    }

    private void OnNewPlayer(IEvent payload)
    {
      ClientVo clientVo =(ClientVo) payload.data;
      
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;
      
      var instantiateAsync = Addressables.InstantiateAsync(LobbyKey.LobbyManagerPanelItem, view.playerContainer);
      instantiateAsync.Completed += handle =>
      {
        GameObject obj = instantiateAsync.Result;
        LobbyManagerPanelItemBehaviour behaviour = obj.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
        behaviour.Init(clientVo,lobbyModel.colors[clientVo.inLobbyId]);
        view.behaviours[clientVo.inLobbyId]=behaviour;
      };    
    }



    private void OnReady()
    {
      dispatcher.Dispatch(LobbyEvent.PlayerReady);
    }
    
    private void OnPlayerReadyResponse(IEvent payload)
    {
      ushort inLobbyId = (ushort)payload.data;
      view.behaviours[inLobbyId].PlayerReady();
      Debug.Log("We got it player is ready");

    }
    
    private void OnBack()
    {
      dispatcher.Dispatch(LobbyEvent.OutLobby);
    }
    private void OnPlayerIsOut(IEvent payload)
    {
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;
      ushort inLobbyId = (ushort)payload.data;
      view.behaviours[inLobbyId].PlayerIsOut();
      for (ushort i = 0; i < lobbyVo.clients.Count; i++)
      {
        view.behaviours[i].Init(lobbyVo.clients[i],lobbyModel.colors[i]);
      }
    }
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Ready,OnReady);
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Back,OnBack);
      dispatcher.RemoveListener(LobbyEvent.NewPlayerToLobby,OnNewPlayer);
      dispatcher.RemoveListener(LobbyEvent.PlayerReadyResponse,OnPlayerReadyResponse);
      dispatcher.RemoveListener(LobbyEvent.PlayerIsOut,OnPlayerIsOut);

    }
  }
}