using System;
using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.Discord.Model;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.MainGameManager
{
  public class MainGameManagerMediator : EventMediator
  {
    [Inject]
    public MainGameManagerView view { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    [Inject]
    public IDiscordModel discordModel { get; set; }
    
    [Inject]
    public IPlayerModel playerModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(ServerToClientId.SendTurn, OnTurn);

      dispatcher.AddListener(MainGameEvent.TurnTimeOver, TurnTimeOver);
    }
    
    private void Start()
    {
      GameStartVo vo = new()
      {
        gameStart = true,
        lobbyCode = lobbyModel.lobbyVo.lobbyCode
      };
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GameStart);
      message = networkManager.SetData(message, vo);
      
      networkManager.Client.Send(message);
      
      discordModel.InGame(playerModel.playerRegisterInfoVo.username);
    }
    
    private void OnTurn(IEvent payload)
    {
      MessageReceivedVo messageReceivedVo = (MessageReceivedVo)payload.data;
      ushort inLobbyId = Convert.ToUInt16(messageReceivedVo.message[1]);
      
      mainGameModel.queue = inLobbyId;

      screenManagerModel.OpenPanel(MainGameKeys.YourTurnPanel, SceneKey.MainGame, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    private void TurnTimeOver()
    {
      screenManagerModel.CloseSpecificPanel(MainGameKeys.YourTurnPanel);

      NextTurnVo vo = new()
      {
        //TODO ŞAFAK
        //currentTurnPlayerLobbyId = lobbyModel.clientVo.inLobbyId,
        lobbyCode = lobbyModel.lobbyVo.lobbyCode
      };
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.NextTurn);
      message = networkManager.SetData(message, vo);

      networkManager.Client.Send(message);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(ServerToClientId.SendTurn, OnTurn);

      dispatcher.RemoveListener(MainGameEvent.TurnTimeOver, TurnTimeOver);
    }
  }
}