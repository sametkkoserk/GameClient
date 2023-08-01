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
using UnityEngine;

namespace Runtime.Contexts.Lobby.Processor
{
  public class QuitFromLobbyDoneProcessor : EventCommand
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
      OutFromLobbyVo outFromLobbyVo = networkManager.GetData<OutFromLobbyVo>(vo.message);
      if (outFromLobbyVo.id == lobbyModel.clientVo.id)
      {
        
        //lobbyModel.LobbyReset();
        screenManagerModel.OpenPanel(LobbyKey.JoinLobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
        discordModel.OnMenu(playerModel.playerRegisterInfoVo.username);
        
        DebugX.Log(DebugKey.Response,"Host exit from lobby message received");
      }
      else
      {
        dispatcher.Dispatch(LobbyEvent.PlayerIsOut, outFromLobbyVo);
        
        discordModel.InLobby(playerModel.playerRegisterInfoVo.username, lobbyModel.lobbyVo.playerCount, lobbyModel.lobbyVo.maxPlayerCount);
        
        DebugX.Log(DebugKey.Response,"Quit from lobby message received");
      }
    }
  }
}