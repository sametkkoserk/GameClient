using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
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
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.NextTurn, OnNextTurn);
      dispatcher.AddListener(MainGameEvent.RemainingTime, OnRemainingTime);
      dispatcher.AddListener(MainGameEvent.OpenMiniGameResultPanel, OnOpenMiniGameResultPanel);
    }

    private void Start()
    {
      mainGameModel.clientVo = lobbyModel.clientVo;
      mainGameModel.playerFeaturesVo = new PlayerFeaturesVo();
      
      screenManagerModel.OpenPanel(MainGameKeys.NextTurnNotificationPanel, SceneKey.MainGame, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
      screenManagerModel.OpenPanel(MainGameKeys.CityMiniInfoPanel, SceneKey.MainGame, LayerKey.SecondLayer, PanelMode.Destroy, PanelType.BottomPanel);
      screenManagerModel.OpenPanel(MainGameKeys.MainHudPanel, SceneKey.MainGame, LayerKey.FourthLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }

    private void OnNextTurn(IEvent payload)
    {
      TurnVo turnVo = (TurnVo)payload.data;
      
      ClientVo client = lobbyModel.lobbyVo.clients[turnVo.id];

      MainHudTurnVo mainHudTurnVo = new()
      {
        title = client.userName,
        color = client.playerColor,
        id = client.id,
        panelTypeKey = NotificationPanelTypeKey.NextTurn,
        time = PanelClosingTimes.nextTurn
      };
      
      dispatcher.Dispatch(MainGameEvent.NotificationPanel, mainHudTurnVo);
    }

    private void OnRemainingTime(IEvent payload)
    {
      TurnVo turnVo = (TurnVo)payload.data;
      
      dispatcher.Dispatch(MainGameEvent.RemainingTimeMainHud, turnVo.remainingTime);
    }

    public void OnOpenMiniGameResultPanel()
    {
      screenManagerModel.OpenPanel(MainGameKeys.MiniGameResultPanel, SceneKey.MainGame, LayerKey.FirstLayer, PanelMode.Additive, PanelType.FullScreenPanel);
    }
    
    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.NextTurn, OnNextTurn);
      dispatcher.RemoveListener(MainGameEvent.RemainingTime, OnRemainingTime);
      dispatcher.RemoveListener(MainGameEvent.OpenMiniGameResultPanel, OnOpenMiniGameResultPanel);
    }
  }
}