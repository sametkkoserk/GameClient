using Runtime.Lobby.Enum;
using Runtime.Lobby.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Lobby.View.JoinLobbyPanel
{
  public enum JoinLobbyPanelEvent
  {
    Back
  }
  public class JoinLobbyPanelMediator : EventMediator
  {
    [Inject]
    public JoinLobbyPanelView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(JoinLobbyPanelEvent.Back,OnBack);
      dispatcher.AddListener(LobbyEvent.listLobbies,OnLobbies);
    }

    private void Start()
    {
      dispatcher.Dispatch(LobbyEvent.GetLobbies);
      Debug.Log("GetLobbies Waiting");
    }
    private void OnLobbies(IEvent payload)
    {
      LobbiesVo vo = (LobbiesVo)payload.data;
      for (int i = 0; i < vo.lobbies.Count; i++)
      {
        int count = i;
        var asyncOperationHandle = Addressables.InstantiateAsync(LobbyKey.JoinLobbyPanelItem,view.lobbyContainer);
        asyncOperationHandle.Completed += handle =>
        {
          GameObject obj = asyncOperationHandle.Result;
          JoinLobbyPanelItemBehaviour behaviour = obj.GetComponent<JoinLobbyPanelItemBehaviour>();
          behaviour.Init(vo.lobbies[count],() => {dispatcher.Dispatch(LobbyEvent.JoinLobby,vo.lobbies[count].lobbyId);});

        };
      }
      
    }
    
    private void OnBack()
    {
      dispatcher.Dispatch(LobbyEvent.BackToLobbyPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(JoinLobbyPanelEvent.Back,OnBack);
      dispatcher.RemoveListener(LobbyEvent.listLobbies,OnLobbies);
    }
  }
}