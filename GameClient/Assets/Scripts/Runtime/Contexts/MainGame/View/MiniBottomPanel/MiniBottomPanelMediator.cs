using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.MiniBottomPanel
{
  public enum MiniBottomPanelEvent
  {
    Pass,
  }
  public class MiniBottomPanelMediator : EventMediator
  {
    [Inject]
    public MiniBottomPanelView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(MiniBottomPanelEvent.Pass, OnPass);
      
      dispatcher.AddListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.AddListener(MainGameEvent.ShowHideMiniBottomPanel, OnShowHidePanel);
      
      Init();
    }

    private void Init()
    {
      gameObject.SetActive(false);
    }

    private void OnNextTurn(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;

      view.operationButton.interactable = mainHudTurnVo.id == lobbyModel.clientVo.id;

      for (int i = 0; i < view.images.Length; i++)
        view.images[i].color = mainHudTurnVo.color.ToColor();

      view.playerName.text = lobbyModel.lobbyVo.clients[mainHudTurnVo.id].userName;
      view.stateText.text = mainGameModel.gameStateKey.ToString();
    }

    private void OnPass()
    {
      dispatcher.Dispatch(MainGameEvent.Pass);
    }

    private void OnShowHidePanel(IEvent payload)
    {
      bool open = (bool)payload.data;

      gameObject.SetActive(open);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Pass, OnPass);

      dispatcher.RemoveListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.RemoveListener(MainGameEvent.ShowHideMiniBottomPanel, OnShowHidePanel);
    }
  }
}