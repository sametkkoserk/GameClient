using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Processor
{
  public class NewPlayerToLobbyProccessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      var vo = (MessageReceivedVo)evt.data;
      var message = vo.message;
      var clientVo = networkManager.GetData<ClientVo>(message);
      // {
      //   id = message.GetUShort(),
      //   inLobbyId = message.GetUShort(),
      //   //userName = message.GetString(),
      //   colorId = message.GetUShort()
      // };
      lobbyModel.lobbyVo.clients[clientVo.inLobbyId] = clientVo;
      lobbyModel.lobbyVo.playerCount += 1;
      dispatcher.Dispatch(LobbyEvent.NewPlayerToLobby, clientVo);
    }
  }
}