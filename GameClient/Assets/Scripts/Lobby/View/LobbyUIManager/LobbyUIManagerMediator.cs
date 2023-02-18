using System;
using Lobby.Enum;
using strange.extensions.mediation.impl;
using UnityEngine.AddressableAssets;

namespace Lobby.View.LobbyUIManager
{
  public class LobbyUIManagerMediator : EventMediator
  {
    [Inject]
    public LobbyUIManagerView view { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(LobbyEvent.ToCreatePanel,OnToCreate);
      dispatcher.AddListener(LobbyEvent.ToJoinPanel,OnToJoin);
      dispatcher.AddListener(LobbyEvent.ToLobbyManagerPanel,OnToLobbyManagerPanel);
      dispatcher.AddListener(LobbyEvent.BackToLobbyPanel,OnBackToLobbyPanel);
      dispatcher.AddListener(LobbyEvent.StartGame,OnDestroyCurrent);
    }


    private void Start()
    {
      OnBackToLobbyPanel();
    }
    private void OnBackToLobbyPanel()
    {
      if (view.CurrentPanel!=null)
      {
        OnDestroyCurrent();
      }
      var asyncop = Addressables.InstantiateAsync(LobbyKey.LobbyPanel, gameObject.transform);
      asyncop.Completed+=handle =>
      {
        view.CurrentPanel=asyncop.Result;
      };    
    }
    
    private void OnToCreate()
    {
      OnDestroyCurrent();
      var asyncop = Addressables.InstantiateAsync(LobbyKey.CreateLobbyPanel, gameObject.transform);
      asyncop.Completed+=handle =>
      {
        view.CurrentPanel=asyncop.Result;
      };
    }
    
    private void OnToJoin()
    {
      OnDestroyCurrent();
      var asyncop = Addressables.InstantiateAsync(LobbyKey.JoinLobbyPanel, gameObject.transform);
      asyncop.Completed+=handle =>
      {
        view.CurrentPanel=asyncop.Result;
      };
      
    }
    
    private void OnToLobbyManagerPanel()
    {
      OnDestroyCurrent();
      var asyncop = Addressables.InstantiateAsync(LobbyKey.LobbyManagerPanel, gameObject.transform);
      asyncop.Completed+=handle =>
      {
        view.CurrentPanel=asyncop.Result;
      };
    }
    
    private void OnDestroyCurrent()
    {
      Destroy(view.CurrentPanel);
      view.CurrentPanel = null;

    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(LobbyEvent.ToCreatePanel,OnToCreate);
      dispatcher.RemoveListener(LobbyEvent.ToJoinPanel,OnToJoin);
      dispatcher.RemoveListener(LobbyEvent.ToLobbyManagerPanel,OnToLobbyManagerPanel);
      dispatcher.RemoveListener(LobbyEvent.BackToLobbyPanel,OnBackToLobbyPanel);
      dispatcher.RemoveListener(LobbyEvent.StartGame,OnDestroyCurrent);

    }
  }
}