using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Lobby.Processor
{
  public class OutFromLobbyDoneProcessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      string message = vo.message;
      ushort inLobbyId = networkManager.GetData<ushort>(message);
      Debug.Log("outed message received");
      if (inLobbyId==lobbyModel.inLobbyId)
      {
        lobbyModel.LobbyReset();
        dispatcher.Dispatch(LobbyEvent.BackToLobbyPanel);

      }
      else
      {
        lobbyModel.OutFromLobby(inLobbyId);
        dispatcher.Dispatch(LobbyEvent.PlayerIsOut,inLobbyId);

      }
      
    }
  }
}