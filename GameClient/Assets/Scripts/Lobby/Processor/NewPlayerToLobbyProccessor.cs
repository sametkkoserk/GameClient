using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;

namespace Lobby.Processor
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
      lobbyModel.lobbyVo.clients.Add(clientVo);
      lobbyModel.lobbyVo.playerCount += 1;
      dispatcher.Dispatch(LobbyEvent.NewPlayerToLobby, clientVo);
    }
  }
}