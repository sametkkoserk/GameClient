using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
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
      ushort inLobbyId = networkManager.GetData<ushort>(vo.message);
      if (inLobbyId == lobbyModel.clientVo.inLobbyId)
      {
        lobbyModel.LobbyReset();
        screenManagerModel.OpenPanel(LobbyKey.LobbyPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
        discordModel.OnMenu(playerModel.playerRegisterInfoVo.username);
      }
      else
      {
        lobbyModel.OutFromLobby(inLobbyId);
        dispatcher.Dispatch(LobbyEvent.PlayerIsOut, inLobbyId);
        
        discordModel.InLobby(playerModel.playerRegisterInfoVo.username, lobbyModel.lobbyVo.playerCount, lobbyModel.lobbyVo.maxPlayerCount);
        
        DebugX.Log(DebugKey.Response,"Quit From Lobby message Received");

      }
    }
  }
}