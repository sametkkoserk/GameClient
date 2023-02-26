using System.Collections.Generic;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.Lobby.View.LobbyManagerPanel
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

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Ready, OnReady);
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Back, OnBack);
      
      dispatcher.AddListener(LobbyEvent.NewPlayerToLobby, OnNewPlayer);
      dispatcher.AddListener(LobbyEvent.PlayerReadyResponse, OnPlayerReadyResponse);
      dispatcher.AddListener(LobbyEvent.PlayerIsOut, OnPlayerIsOut);
    }


    private void Start()
    {
      view.behaviours = new Dictionary<ushort, LobbyManagerPanelItemBehaviour>();
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.lobbyNameText.text = lobbyVo.lobbyName;
      view.playerCountText.text = $"{lobbyVo.playerCount}/{lobbyVo.maxPlayerCount}";
      for (ushort i = 0; i < lobbyVo.playerCount; i++)
      {
        ushort count = i;

        GameObject instantiatedGameObject = Instantiate(view.lobbyManagerPanelItem, view.playerContainer);
        ClientVo clientVo = lobbyVo.clients[count];
        LobbyManagerPanelItemBehaviour behaviour = instantiatedGameObject.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
        behaviour.Init(clientVo, lobbyModel.colors[clientVo.inLobbyId]);

        view.behaviours[clientVo.inLobbyId] = behaviour;
      }
    }

    private void OnNewPlayer(IEvent payload)
    {
      ClientVo clientVo = (ClientVo)payload.data;

      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;

      GameObject instantiatedGameObject = Instantiate(view.lobbyManagerPanelItem, view.playerContainer);
      LobbyManagerPanelItemBehaviour behaviour = instantiatedGameObject.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
      behaviour.Init(clientVo, lobbyModel.colors[clientVo.inLobbyId]);
      view.behaviours[clientVo.inLobbyId] = behaviour;
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
        view.behaviours[i].Init(lobbyVo.clients[i], lobbyModel.colors[i]);
      }
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Ready, OnReady);
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Back, OnBack);
      dispatcher.RemoveListener(LobbyEvent.NewPlayerToLobby, OnNewPlayer);
      dispatcher.RemoveListener(LobbyEvent.PlayerReadyResponse, OnPlayerReadyResponse);
      dispatcher.RemoveListener(LobbyEvent.PlayerIsOut, OnPlayerIsOut);
    }
  }
}