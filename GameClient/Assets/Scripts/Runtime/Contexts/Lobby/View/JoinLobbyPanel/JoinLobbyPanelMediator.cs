using System.Collections.Generic;
using System.Linq;
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
    Back
  }

  public class JoinLobbyPanelMediator : EventMediator
  {
    [Inject]
    public JoinLobbyPanelView view { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    private void Start()
    {
      dispatcher.Dispatch(LobbyEvent.GetLobbies);
      Debug.Log("GetLobbies Waiting");
    }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(JoinLobbyPanelEvent.Back, OnBack);

      dispatcher.AddListener(LobbyEvent.listLobbies, OnLobbies);
    }

    private void OnLobbies(IEvent payload)
    {
      Dictionary<string, LobbyVo> lobbies = (Dictionary<string, LobbyVo>)payload.data;
      for (ushort i = 0; i < lobbies.Count; i++)
      {
        ushort count = i;
        GameObject joinLobbyPanelItem = Instantiate(view.joinLobbyPanelItem, view.lobbyContainer);
        JoinLobbyPanelItemBehaviour behaviour = joinLobbyPanelItem.GetComponent<JoinLobbyPanelItemBehaviour>();
        behaviour.Init(lobbies.ElementAt(count).Value, () =>
        {
          dispatcher.Dispatch(LobbyEvent.JoinLobby, lobbies.ElementAt(count).Value.lobbyCode);
        });
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