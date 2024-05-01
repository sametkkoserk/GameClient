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
  public class NewPlayerToLobbyProcessor : EventCommand
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
      JoinedToLobbyVo joinedToLobbyVo = networkManager.GetData<JoinedToLobbyVo>(vo.message);
      
      lobbyModel.SetLobbyVo(joinedToLobbyVo.lobbyVo);
      dispatcher.Dispatch(LobbyEvent.NewPlayerToLobby);
      
      discordModel.InLobby(playerModel.player.username, lobbyModel.lobbyVo.playerCount, lobbyModel.lobbyVo.maxPlayerCount);

      DebugX.Log(DebugKey.Response,"New Player message Received");
    }
  }
}