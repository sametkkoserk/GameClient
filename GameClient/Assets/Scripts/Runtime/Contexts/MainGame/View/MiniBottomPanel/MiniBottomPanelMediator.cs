using System.Collections.Generic;
using System.Linq;
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
    Increment,
    Decrement,
    Confirm
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
      view.dispatcher.AddListener(MiniBottomPanelEvent.Increment, OnIncrement);
      view.dispatcher.AddListener(MiniBottomPanelEvent.Decrement, OnDecrement);
      view.dispatcher.AddListener(MiniBottomPanelEvent.Confirm, OnConfirm);
      
      dispatcher.AddListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.AddListener(MainGameEvent.ShowHideMiniBottomPanel, OnShowHidePanel);
      dispatcher.AddListener(MainGameEvent.SetTransferSoldierAfterAttack, OnSelectSoldierTransferCount);
      
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
      view.operationButton.interactable = false;
      
      dispatcher.Dispatch(MainGameEvent.Pass);
    }

    private void OnShowHidePanel(IEvent payload)
    {
      bool open = (bool)payload.data;

      gameObject.SetActive(open);
      OnShowHidePanelContent(!open);
    }

    private void OnShowHidePanelContent(bool tf)
    {
      view.passButtonPart.gameObject.SetActive(!tf);

      if (mainGameModel.queueID != lobbyModel.clientVo.id)
        tf = false;

      for (int i = 0; i < view.fortifyPart.Count; i++)
      {
        view.fortifyPart.ElementAt(i).gameObject.SetActive(tf);
      }

      for (int i = 0; i < view.changeCountButtons.Count; i++)
      {
        view.changeCountButtons.ElementAt(i).gameObject.SetActive(tf);
        view.changeCountButtons.ElementAt(i).interactable = tf;
      }

      view.confirmButton.interactable = tf;
      view.confirmButton.gameObject.SetActive(tf);
    }

    private void OnSelectSoldierTransferCount(IEvent payload)
    {
      AttackResultVo attackResultVo = (AttackResultVo)payload.data;
      view.maxSoldierCount = attackResultVo.winnerCity.soldierCount - 1;
      
      view.cityIDs = new KeyValuePair<int, int>(attackResultVo.winnerCity.ID, attackResultVo.loserCity.ID);
      
      if(view.maxSoldierCount <= 0)
        return;
      
      OnShowHidePanelContent(true);

      view.soldierCountInPanel = 1;
      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnIncrement()
    {
      view.soldierCountInPanel++;

      if (view.soldierCountInPanel > view.maxSoldierCount)
      {
        view.soldierCountInPanel = 1;
      }

      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnDecrement()
    {
      view.soldierCountInPanel--;

      if (view.soldierCountInPanel < 1)
      {
        view.soldierCountInPanel = view.maxSoldierCount;
      }

      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnConfirm()
    {
      OnShowHidePanelContent(false);
      gameObject.SetActive(true);
      
      FortifyVo fortifyVo = new()
      {
        sourceCityId = view.cityIDs.Key,
        targetCityId = view.cityIDs.Value,
        soldierCount = view.soldierCountInPanel
      };
      
      dispatcher.Dispatch(MainGameEvent.ConfirmFortify, fortifyVo);
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Pass, OnPass);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Increment, OnIncrement);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Decrement, OnDecrement);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Confirm, OnConfirm);

      dispatcher.RemoveListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.RemoveListener(MainGameEvent.ShowHideMiniBottomPanel, OnShowHidePanel);
      dispatcher.RemoveListener(MainGameEvent.SetTransferSoldierAfterAttack, OnSelectSoldierTransferCount);
    }
  }
}