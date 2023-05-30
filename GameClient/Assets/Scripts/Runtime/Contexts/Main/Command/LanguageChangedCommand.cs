using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Modules.Core.Discord.Model;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class LanguageChangedCommand : EventCommand
  {
    [Inject]
    public IDiscordModel discordModel { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void Execute()
    {
      switch (discordModel.lastState)
      {
        case 0:
          discordModel.StarterSettings();
          break;
        case 1:
          discordModel.OnMenu(playerModel.playerRegisterInfoVo.username);
          break;
        case 2:
          discordModel.InLobby(playerModel.playerRegisterInfoVo.username, lobbyModel.lobbyVo.playerCount, lobbyModel.lobbyVo.maxPlayerCount);
          break;
        case 3:
          discordModel.InGame(playerModel.playerRegisterInfoVo.username);
          break;
      }
    }
  }
}