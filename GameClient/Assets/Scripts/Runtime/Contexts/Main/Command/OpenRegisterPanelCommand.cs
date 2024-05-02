using System.Collections.Generic;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Modules.Core.Discord.Model;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Main.Command
{
  public class OpenRegisterPanelCommand : EventCommand
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public IDiscordModel discordModel { get; set; }

    public override void Execute()
    {
      if (PlayerPrefs.HasKey("username")&&PlayerPrefs.HasKey("password"))
      {

        var map = new Dictionary<string, object>() { { "username",  PlayerPrefs.GetString("username") },{ "password",  PlayerPrefs.GetString("password") } };
        
        ApiManagerService.instance.Request<PlayerVo>("/auth/login",RequestType.POST,map, res =>
        {
          if (res.Error==null)
          {
            PlayerRegisterInfoVo registerVo = new PlayerRegisterInfoVo()
            {
              username = res.Result.username,
              email = res.Result.email,
            };
            dispatcher.Dispatch(MainEvent.RegisterInfoSend, registerVo);

            // playerModel.player = res.Result;
            // screenManagerModel.CloseAllPanels();
            // crossDispatcher.Dispatch(LobbyEvent.LoginOrRegisterCompletedSuccessfully);
            //
            // discordModel.OnMenu(playerModel.player.username);
          }
        });
      }
      else
      {
        screenManagerModel.OpenPanel(MainPanelKey.RegisterPanel, SceneKey.Main, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
      }
    }
  }
}