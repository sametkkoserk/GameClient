using Riptide;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class NewPlayerToLobbyProccessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel{ get; set; }
    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Message message = vo.message;
      ClientVo clientVo = new ClientVo()
      {
        id = message.GetUShort(),
        inLobbyId = message.GetUShort(),
        //userName = message.GetString(),
        colorId = message.GetUShort()
      };
      lobbyModel.lobbyVo.clients[clientVo.inLobbyId]=clientVo;
      lobbyModel.lobbyVo.playerCount += 1;
      dispatcher.Dispatch(LobbyEvent.NewPlayerToLobby, clientVo);
    }
  }
}