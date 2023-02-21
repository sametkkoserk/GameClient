using Riptide;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Vo;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Lobby.Processor
{
  public class NewPlayerToLobbyProccessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel{ get; set; }
    
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      string message = vo.message;
      ClientVo clientVo = networkManager.GetData<ClientVo>(message);
      // {
      //   id = message.GetUShort(),
      //   inLobbyId = message.GetUShort(),
      //   //userName = message.GetString(),
      //   colorId = message.GetUShort()
      // };
      lobbyModel.lobbyVo.clients[clientVo.inLobbyId]=clientVo;
      lobbyModel.lobbyVo.playerCount += 1;
      dispatcher.Dispatch(LobbyEvent.NewPlayerToLobby, clientVo);
    }
  }
}