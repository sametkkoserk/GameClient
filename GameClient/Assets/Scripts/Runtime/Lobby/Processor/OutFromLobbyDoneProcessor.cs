using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Lobby.Processor
{
  public class OutFromLobbyDoneProcessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Message message = vo.message;
      ushort inLobbyId = message.GetUShort();
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