using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Processor
{
  public class GetGameSettingsProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      var vo = (MessageReceivedVo)evt.data;
      var lobbySettingsVo = networkManager.GetData<LobbySettingsVo>(vo.message);

      lobbyModel.lobbyVo.lobbySettingsVo = lobbySettingsVo;
      dispatcher.Dispatch(LobbyEvent.GetGameSettings);
    }
  }
}