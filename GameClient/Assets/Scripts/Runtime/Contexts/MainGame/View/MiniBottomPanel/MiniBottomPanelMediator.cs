using System.Collections.Generic;
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
    Confirm,
    CloseSoldierSelectorPanel
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
      view.dispatcher.AddListener(MiniBottomPanelEvent.CloseSoldierSelectorPanel, OnCloseSoldierSelectorPanel);
      
      dispatcher.AddListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.AddListener(MainGameEvent.GameStateChanged, OnGameStateChanged);
      dispatcher.AddListener(MainGameEvent.ShowSelectorPartInBottomPanel, OnOpenSoldierCountSelector);
      dispatcher.AddListener(MainGameEvent.DisappearBottomPanel, OnDisappearBottomPanel);
      dispatcher.AddListener(MainGameEvent.SetTransferSoldierAfterAttack, OnSelectSoldierTransferCountForAttackResult);
      dispatcher.AddListener(MainGameEvent.SetTransferSoldierForFortify, OnSelectSoldierTransferCountForFortify);
      
      Init();
    }

    private void Init()
    {
      OnDisappearBottomPanel();
    }

    private void OnNextTurn(IEvent payload)
    {
      MainHudTurnVo mainHudTurnVo = (MainHudTurnVo)payload.data;

      view.passButton.interactable = mainHudTurnVo.id == lobbyModel.clientVo.id;

      view.backgroundColor.color = mainHudTurnVo.color.ToColor();

      view.playerName.text = lobbyModel.lobbyVo.clients[mainHudTurnVo.id].userName;
      view.stateText.text = mainGameModel.gameStateKey.ToString();

      OnOpenPassPart();
    }

    private void OnGameStateChanged()
    {
      view.stateText.text = mainGameModel.gameStateKey.ToString();
      
      view.passButton.interactable = mainGameModel.queueID == lobbyModel.clientVo.id;
    }

    private void OnPass()
    {
      if (mainGameModel.gameStateKey == GameStateKey.Fortify)
      {
        OnDisappearBottomPanel();
      }
      view.passButton.interactable = false;
      
      dispatcher.Dispatch(MainGameEvent.Pass);
    }

    private void OnOpenPassPart()
    {
      view.soldierSelector.SetActive(false);
      view.passButtonPart.SetActive(true);

      view.stateText.text = mainGameModel.gameStateKey.ToString();
      view.passButton.interactable = mainGameModel.queueID == lobbyModel.clientVo.id;
    }

    private void OnOpenSoldierCountSelector()
    {
      if (mainGameModel.queueID != lobbyModel.clientVo.id) return;
      
      switch (mainGameModel.gameStateKey)
      {
        case GameStateKey.Arming:
          PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;

          if (playerFeaturesVo.freeSoldierCount <= 0 && mainGameModel.cities[mainGameModel.selectedCityId].ownerID != mainGameModel.clientVo.id)
            return;
          
          view.maxSoldierCount = mainGameModel.playerFeaturesVo.freeSoldierCount;
          break;
      }
      
      view.passButtonPart.SetActive(false);
      view.soldierSelector.SetActive(true);

      view.soldierCountInPanel = 1;
      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnDisappearBottomPanel()
    {
      view.passButtonPart.SetActive(false);
      view.soldierSelector.SetActive(false);
    }

    private void OnSelectSoldierTransferCountForAttackResult(IEvent payload)
    {
      AttackResultVo attackResultVo = (AttackResultVo)payload.data;
      view.maxSoldierCount = attackResultVo.winnerCity.soldierCount - 1;
      
      view.cityIDs = new KeyValuePair<int, int>(attackResultVo.winnerCity.ID, attackResultVo.loserCity.ID);
      
      if(view.maxSoldierCount <= 0)
        return;
      
      OnOpenSoldierCountSelector();

      view.soldierCountInPanel = 1;
      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }
    
    private void OnSelectSoldierTransferCountForFortify(IEvent payload)
    {
      FortifyVo fortifyVo = (FortifyVo)payload.data;
      view.maxSoldierCount = mainGameModel.cities[fortifyVo.sourceCityId].soldierCount - 1;
      
      view.cityIDs = new KeyValuePair<int, int>(fortifyVo.sourceCityId, fortifyVo.targetCityId);
      
      if(view.maxSoldierCount <= 0)
        return;
      
      OnOpenSoldierCountSelector();

      view.soldierCountInPanel = 1;
      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnConfirm()
    {
      if (mainGameModel.gameStateKey == GameStateKey.Arming)
      {
        OnConfirmArming();
      }
      else
      {
        FortifyVo fortifyVo = new()
        {
          sourceCityId = view.cityIDs.Key,
          targetCityId = view.cityIDs.Value,
          soldierCount = view.soldierCountInPanel
        };
      
        dispatcher.Dispatch(MainGameEvent.ConfirmFortify, fortifyVo);
      }
      
      OnOpenPassPart();
    }
    
    private void OnConfirmArming()
    {
      PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;

      if (playerFeaturesVo.freeSoldierCount <= 0)
        return;

      if (view.soldierCountInPanel > playerFeaturesVo.freeSoldierCount)
        view.soldierCountInPanel = playerFeaturesVo.freeSoldierCount;

      if (view.soldierCountInPanel < 1)
        view.soldierCountInPanel = 1;

      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      ArmingVo armingVo = new()
      {
        cityID = cityVo.ID,
        soldierCount = view.soldierCountInPanel
      };

      dispatcher.Dispatch(MainGameEvent.ArmingToCity, armingVo);
    }
    
    private void OnIncrement()
    {
      view.soldierCountInPanel++;

      if (view.soldierCountInPanel > view.maxSoldierCount)
        view.soldierCountInPanel = 1;

      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnDecrement()
    {
      view.soldierCountInPanel--;

      if (view.soldierCountInPanel < 1)
        view.soldierCountInPanel = view.maxSoldierCount;

      view.soldierCountText.text = view.soldierCountInPanel.ToString();
    }

    private void OnCloseSoldierSelectorPanel()
    {
      OnOpenPassPart();
    }
    
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Pass, OnPass);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Increment, OnIncrement);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Decrement, OnDecrement);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.Confirm, OnConfirm);
      view.dispatcher.RemoveListener(MiniBottomPanelEvent.CloseSoldierSelectorPanel, OnCloseSoldierSelectorPanel);

      dispatcher.RemoveListener(MainGameEvent.NextTurnMainHud, OnNextTurn);
      dispatcher.RemoveListener(MainGameEvent.ShowSelectorPartInBottomPanel, OnOpenSoldierCountSelector);
      dispatcher.RemoveListener(MainGameEvent.DisappearBottomPanel, OnDisappearBottomPanel);
      dispatcher.RemoveListener(MainGameEvent.SetTransferSoldierAfterAttack, OnSelectSoldierTransferCountForAttackResult);
      dispatcher.RemoveListener(MainGameEvent.SetTransferSoldierForFortify, OnSelectSoldierTransferCountForFortify);
    }
  }
}