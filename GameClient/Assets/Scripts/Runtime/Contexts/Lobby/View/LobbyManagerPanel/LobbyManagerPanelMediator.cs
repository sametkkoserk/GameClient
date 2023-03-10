using System.Collections.Generic;
using System.Linq;
using Riptide;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.View.LobbyManagerPanel
{
  public enum LobbyManagerPanelEvent
  {
    Ready,
    Back,
    
    // Game Settings
    Save,
    ChangedSettings
  }

  public class LobbyManagerPanelMediator : EventMediator
  {
    [Inject]
    public LobbyManagerPanelView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Ready, OnReady);
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Back, OnBack);
      view.dispatcher.AddListener(LobbyManagerPanelEvent.Save, OnSave);
      view.dispatcher.AddListener(LobbyManagerPanelEvent.ChangedSettings, OnChangedSettings);
      
      dispatcher.AddListener(LobbyEvent.NewPlayerToLobby, OnNewPlayer);
      dispatcher.AddListener(LobbyEvent.PlayerReadyResponse, OnPlayerReadyResponse);
      dispatcher.AddListener(LobbyEvent.PlayerIsOut, OnPlayerIsOut);
      
      dispatcher.AddListener(ServerToClientId.GameSettingsChanged, OnGetGameSettings);
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

      InitLobbySettings();
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
      view.ready = !view.ready;
      dispatcher.Dispatch(view.ready ? LobbyEvent.PlayerReady : LobbyEvent.PlayerUnready);
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
      ushort inLobbyId = (ushort)payload.data;
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      
      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;
      
      view.behaviours[inLobbyId].PlayerIsOut();
      for (ushort i = 0; i < lobbyVo.clients.Count; i++)
      {
        view.behaviours[i].Init(lobbyVo.clients[i], lobbyModel.colors[i]);
      }
    }

    #region Game Settings
    private void InitLobbySettings()
    {
      ushort leaderId = lobbyModel.lobbyVo.leaderId;
      
      for (int i = 0; i < view.adminGameObjects.Count; i++)
      {
        view.adminGameObjects[i].gameObject.SetActive(lobbyModel.clientVo.id == leaderId);
      }
      
      for (int i = 0; i < view.playerGameObjects.Count; i++)
      {
        view.playerGameObjects[i].gameObject.SetActive(lobbyModel.clientVo.id != leaderId);
      }
      
      view.saveButton.interactable = view.changedSettings;

      view.lobbySettingsVo = new LobbySettingsVo
      {
        turnTime = 60,
      };
    }
    private void OnSave()
    {
      if (!view.changedSettings)
        return;

      view.changedSettings = false;
      view.saveButton.interactable = view.changedSettings;
      
      view.lobbySettingsVo.turnTime = float.Parse(view.timerDropdown.options[view.timerDropdown.value].text);
      
      dispatcher.Dispatch(LobbyEvent.GameSettingsChanged, view.lobbySettingsVo);
    }
    
    private void OnGetGameSettings(IEvent payload)
    {
      LobbySettingsVo lobbySettingsVo = (LobbySettingsVo)payload.data;

      view.turnTimerText.text = lobbySettingsVo.turnTime.ToString("f0");
    }
    
    private void OnChangedSettings()
    {
      view.changedSettings = true;
      view.saveButton.interactable = view.changedSettings;
    }
    
    #endregion

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Ready, OnReady);
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Back, OnBack);
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.Save, OnSave);
      view.dispatcher.RemoveListener(LobbyManagerPanelEvent.ChangedSettings, OnChangedSettings);

      dispatcher.RemoveListener(LobbyEvent.NewPlayerToLobby, OnNewPlayer);
      dispatcher.RemoveListener(LobbyEvent.PlayerReadyResponse, OnPlayerReadyResponse);
      dispatcher.RemoveListener(LobbyEvent.PlayerIsOut, OnPlayerIsOut);
      
      dispatcher.RemoveListener(ServerToClientId.GameSettingsChanged, OnGetGameSettings);
    }
  }
}