using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.Discord.Model;
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
    
    [Inject]
    public IDiscordModel discordModel { get; set; }
    
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      ClientVo clientVo = networkManager.GetData<ClientVo>(vo.message);
      // {
      //   id = message.GetUShort(),
      //   inLobbyId = message.GetUShort(),
      //   //userName = message.GetString(),
      //   colorId = message.GetUShort()
      // };
      lobbyModel.lobbyVo.clients[clientVo.inLobbyId] = clientVo;
      lobbyModel.lobbyVo.playerCount += 1;
      dispatcher.Dispatch(LobbyEvent.NewPlayerToLobby, clientVo);
      
      discordModel.InLobby(playerModel.playerRegisterInfoVo.username, lobbyModel.lobbyVo.playerCount, lobbyModel.lobbyVo.maxPlayerCount);

      DebugX.Log(DebugKey.Response,"New Player message Received");
    }
  }
}