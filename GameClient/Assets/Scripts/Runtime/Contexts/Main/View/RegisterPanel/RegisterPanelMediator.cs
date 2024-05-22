using System;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Modules.Core.Discord.Model;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Contexts.Main.View.RegisterPanel
{
  public enum RegisterPanelEvent
  {
    Register,
    Login
  }
  public class RegisterPanelMediator : EventMediator
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    [Inject]
    public RegisterPanelView view { get; set; }

    [Inject]
    public IPlayerModel playerModel { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    [Inject]
    public IDiscordModel discordModel { get; set; }
    
    public override void OnRegister()
    {
      view.dispatcher.AddListener(RegisterPanelEvent.Register, OnRegisterToGame);
      view.dispatcher.AddListener(RegisterPanelEvent.Login, OnLogin);
    }
    private void OnRegisterToGame(IEvent payload)
    {
      PlayerRegisterInfoVo registerInfoVo = (PlayerRegisterInfoVo)payload.data;
      var map = new Dictionary<string, object>() { { "username",  registerInfoVo.username },{ "email",  registerInfoVo.email },{ "password", registerInfoVo.password } };

      ApiManagerService.instance.Request<PlayerVo>("/User/register",RequestType.POST,map, res =>
      {
        if (res.Error==null)
        {
          PlayerPrefs.SetString("username",registerInfoVo.username);
          PlayerPrefs.SetString("password",registerInfoVo.password);

          playerModel.player = res.Result;
          screenManagerModel.CloseAllPanels();
          crossDispatcher.Dispatch(LobbyEvent.LoginOrRegisterCompletedSuccessfully);

          discordModel.OnMenu(playerModel.player.username);
        }
      });
      dispatcher.Dispatch(MainEvent.RegisterInfoSend, registerInfoVo);
    }
    private void OnLogin()
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

          playerModel.player = res.Result;
          screenManagerModel.CloseAllPanels();
          crossDispatcher.Dispatch(LobbyEvent.LoginOrRegisterCompletedSuccessfully);
            
          discordModel.OnMenu(playerModel.player.username);
        }
        else
        {
          view.errorLoginText.text=LocalizationManager.Localize("RegisterPanelWrongLogin");
        }
      });
    }
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(RegisterPanelEvent.Register, OnRegisterToGame);
      view.dispatcher.RemoveListener(RegisterPanelEvent.Login, OnLogin);

    }
  }
}