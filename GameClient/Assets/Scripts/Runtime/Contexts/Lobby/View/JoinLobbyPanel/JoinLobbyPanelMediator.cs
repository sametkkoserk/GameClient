using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.View.JoinLobbyPanel
{
  public enum JoinLobbyPanelEvent
  {
    Back,
    RefreshList,
    Join
  }

  public class JoinLobbyPanelMediator : EventMediator
  {
    [Inject]
    public JoinLobbyPanelView view { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(JoinLobbyPanelEvent.Back, OnBack);
      view.dispatcher.AddListener(JoinLobbyPanelEvent.RefreshList, OnRefreshList);
      view.dispatcher.AddListener(JoinLobbyPanelEvent.Join, OnJoinLobby);
      
      dispatcher.AddListener(LobbyEvent.listLobbies, OnLobbies);
      dispatcher.AddListener(LobbyEvent.RefreshLobbyList, OnRefreshList);
    }

    private void Start()
    {
      OnRefreshList();
    }
    
    private void OnJoinLobby(IEvent payload)
    {
      string lobbyCode = (string)payload.data;
      dispatcher.Dispatch(LobbyEvent.JoinLobby, lobbyCode);
    }

    private void OnRefreshList()
    {
      view.lobbyListLoadingIcon.SetActive(true);

      dispatcher.Dispatch(LobbyEvent.GetLobbies);
      
      DebugX.Log(DebugKey.Request, "Getting lobbies list.");
    }

    private void OnLobbies(IEvent payload)
    {
      Dictionary<string, LobbyVo> lobbies = (Dictionary<string, LobbyVo>)payload.data;
      view.lobbies = lobbies.Values.ToList();
      view.scroller.ReloadData();
      view.lobbyListLoadingIcon.SetActive(false);
    }

    private void OnBack()
    {
      screenManagerModel.OpenPanel(LobbyKey.LobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(JoinLobbyPanelEvent.Back, OnBack);
      view.dispatcher.RemoveListener(JoinLobbyPanelEvent.RefreshList, OnRefreshList);
      view.dispatcher.RemoveListener(JoinLobbyPanelEvent.Join, OnJoinLobby);
      
      dispatcher.RemoveListener(LobbyEvent.listLobbies, OnLobbies);
      dispatcher.RemoveListener(LobbyEvent.RefreshLobbyList, OnRefreshList);

    }
  }
}