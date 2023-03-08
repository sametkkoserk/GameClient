using System;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerMediator : EventMediator
  {
    [Inject]
    public MainGameManagerView view { get; set; }
    
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(ServerToClientId.SendTurn, OnTurn);
    }
    
    private void Start()
    {
      GameStartVo vo = new()
      {
        gameStart = true,
        lobbyId = lobbyModel.lobbyVo.lobbyId
      };
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GameStart);
      message = networkManager.SetData(message, vo);
      
      networkManager.Client.Send(message);
    }
    
    private void OnTurn(IEvent payload)
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)payload.data;
      ushort inLobbyId = Convert.ToUInt16(messageReceivedVo.message);
      mainGameModel.queue = inLobbyId;
      
      Debug.Log("------------- " + inLobbyId);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(ServerToClientId.SendTurn, OnTurn);
    }
  }
}