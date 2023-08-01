using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
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
      dispatcher.AddListener(LobbyEvent.GetGameSettings, OnGetSettings);
    }
    
    private void Start()
    {
      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.lobbyNameText.text = lobbyVo.lobbyName;
      view.playerCountText.text = $"{lobbyVo.playerCount} / {lobbyVo.maxPlayerCount}";
      for (ushort i = 0; i < lobbyVo.playerCount; i++)
      {
        ushort count = i;

        GameObject instantiatedGameObject = Instantiate(view.lobbyManagerPanelItem, view.playerContainer);
        ClientVo clientVo = lobbyVo.clients.ElementAt(count).Value;
        LobbyManagerPanelItemBehaviour behaviour = instantiatedGameObject.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
        behaviour.Init(clientVo, clientVo.playerColor.ToColor());

        lobbyModel.lobbyVo.clients[clientVo.inLobbyId].lobbyItemBehaviour = behaviour;
      }

      DebugX.Log(DebugKey.JoinServer, $"Player ID: {lobbyModel.clientVo.id}, Player's Lobby ID: {lobbyModel.clientVo.inLobbyId}," +
                                      $" Lobby Code: {lobbyVo.lobbyCode}");

      InitLobbySettings();
    }

    private void OnNewPlayer(IEvent payload)
    {
      ClientVo clientVo = (ClientVo)payload.data;

      LobbyVo lobbyVo = lobbyModel.lobbyVo;
      view.playerCountText.text = $"{lobbyVo.playerCount} / {lobbyVo.maxPlayerCount}";

      GameObject instantiatedGameObject = Instantiate(view.lobbyManagerPanelItem, view.playerContainer);
      LobbyManagerPanelItemBehaviour behaviour = instantiatedGameObject.transform.GetComponent<LobbyManagerPanelItemBehaviour>();
      lobbyModel.lobbyVo.clients[clientVo.inLobbyId].lobbyItemBehaviour = behaviour;

      behaviour.Init(clientVo, clientVo.playerColor.ToColor());
    }

    private void OnReady()
    {
      view.ready = !view.ready;
      dispatcher.Dispatch(view.ready ? LobbyEvent.PlayerReady : LobbyEvent.PlayerUnready);
    }

    private void OnPlayerReadyResponse(IEvent payload)
    {
      ushort inLobbyId = (ushort)payload.data;
      lobbyModel.lobbyVo.clients[inLobbyId].lobbyItemBehaviour.PlayerReady();
      Debug.Log("Player is ready: " + inLobbyId);
    }

    private void OnBack()
    {
      dispatcher.Dispatch(LobbyEvent.QuitLobby);
    }

    private void OnPlayerIsOut(IEvent payload)
    {
      ushort inLobbyId = (ushort)payload.data;
      LobbyVo lobbyVo = lobbyModel.lobbyVo;

      view.playerCountText.text = lobbyVo.playerCount + "/" + lobbyVo.maxPlayerCount;

      lobbyModel.lobbyVo.clients[inLobbyId].lobbyItemBehaviour.PlayerIsOut();
      // for (ushort i = 0; i < lobbyVo.clients.Count; i++)
      //   lobbyModel.userItemBehaviours[i].Init(lobbyVo.clients.ElementAt(i).Value, lobbyModel.colors[i]);
    }

    #region Game Settings

    private void InitLobbySettings()
    {
      for (int i = 0; i < view.adminGameObjects.Count; i++)
        view.adminGameObjects[i].gameObject.SetActive(lobbyModel.clientVo.id == lobbyModel.lobbyVo.leaderId);

      for (int i = 0; i < view.playerGameObjects.Count; i++)
        view.playerGameObjects[i].gameObject.SetActive(lobbyModel.clientVo.id != lobbyModel.lobbyVo.leaderId);

      view.saveButton.interactable = false;

      OnGetSettings();
    }

    private void OnGetSettings()
    {
      // ----- Turn Timer
      view.turnTimerText.text = lobbyModel.lobbyVo.lobbySettingsVo.turnTime.ToString("f0");

      for (int i = 0; i < view.timerDropdown.options.Count; i++)
        if (view.timerDropdown.options[i].text == lobbyModel.lobbyVo.lobbySettingsVo.turnTime.ToString("f0"))
          view.timerDropdown.value = i;
      // Turn Timer -----
    }

    private void OnSave()
    {
      if (lobbyModel.clientVo.id != lobbyModel.lobbyVo.leaderId || !view.changedSettings)
        return;

      view.changedSettings = false;
      view.saveButton.interactable = view.changedSettings;

      LobbySettingsVo newLobbySettingsVo = new()
      {
        lobbyCode = lobbyModel.lobbyVo.lobbyCode,
        turnTime = float.Parse(view.timerDropdown.options[view.timerDropdown.value].text)
      };

      dispatcher.Dispatch(LobbyEvent.GameSettingsChanged, newLobbySettingsVo);
    }

    private void OnChangedSettings()
    {
      if (lobbyModel.lobbyVo.lobbySettingsVo.turnTime.ToString("f0") == view.timerDropdown.options[view.timerDropdown.value].text)
        return;

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
      dispatcher.RemoveListener(LobbyEvent.GetGameSettings, OnGetSettings);
    }
  }
}