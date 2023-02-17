using Lobby.Enum;
using Lobby.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Lobby.View.JoinLobbyPanel
{
  public class JoinLobbyPanelMediator : EventMediator
  {
    [Inject]
    public JoinLobbyPanelView view { get; set; }

    public override void OnRegister()
    {
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
          Debug.Log(count+"   "+vo.lobbies.Count);
          behaviour.Init(vo.lobbies[count],() => {dispatcher.Dispatch(LobbyEvent.JoinLobby,vo.lobbies[count].lobbyId);});

        };
      }
      
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LobbyEvent.listLobbies,OnLobbies);
    }
  }
}