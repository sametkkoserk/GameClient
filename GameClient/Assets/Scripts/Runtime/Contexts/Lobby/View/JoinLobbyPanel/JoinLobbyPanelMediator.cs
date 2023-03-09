using System.Collections.Generic;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.Lobby.View.JoinLobbyPanel
{
  public enum JoinLobbyPanelEvent
  {
    Back
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
      
      dispatcher.AddListener(LobbyEvent.listLobbies, OnLobbies);
    }

    private void Start()
    {
      dispatcher.Dispatch(LobbyEvent.GetLobbies);
      Debug.Log("GetLobbies Waiting");
    }

    private void OnLobbies(IEvent payload)
    {
      Dictionary<ushort, LobbyVo> lobbies = (Dictionary<ushort, LobbyVo>)payload.data;
      for (ushort i = 0; i < lobbies.Count; i++)
      {
        ushort count = i;
        GameObject joinLobbyPanelItem = Instantiate(view.joinLobbyPanelItem, view.lobbyContainer);
        JoinLobbyPanelItemBehaviour behaviour = joinLobbyPanelItem.GetComponent<JoinLobbyPanelItemBehaviour>();
        behaviour.Init(lobbies[count], () => { dispatcher.Dispatch(LobbyEvent.JoinLobby, lobbies[count].lobbyId); });
      }
    }

    private void OnBack()
    {
      screenManagerModel.OpenPanel(LobbyKey.LobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(JoinLobbyPanelEvent.Back, OnBack);
      
      dispatcher.RemoveListener(LobbyEvent.listLobbies, OnLobbies);
    }
  }
}