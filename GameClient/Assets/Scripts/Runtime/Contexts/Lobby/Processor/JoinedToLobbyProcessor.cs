using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.Discord.Model;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Lobby.Processor
{
  public class JoinedToLobbyProcessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    [Inject]
    public IDiscordModel discordModel { get; set; }
    
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      JoinedToLobbyVo joinedToLobbyVo = networkManager.GetData<JoinedToLobbyVo>(vo.message);

      lobbyModel.lobbyVo = joinedToLobbyVo.lobbyVo;
      lobbyModel.clientVo = joinedToLobbyVo.clientVo;

      screenManagerModel.OpenPanel(LobbyKey.LobbyManagerPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
      
      DebugX.Log(DebugKey.Response,"Joined To Lobby message Received");

      discordModel.InLobby(playerModel.playerRegisterInfoVo.username, joinedToLobbyVo.lobbyVo.playerCount, joinedToLobbyVo.lobbyVo.maxPlayerCount);
    }
  }
}