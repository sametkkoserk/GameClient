using System;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public enum CityDetailsPanelEvent
  {
    ChangeArmingCount,
    CloseArmingPanel,
    OnConfirmArming,
    OnDoOperation,
    ClosePanel
  }

  public class CityDetailsPanelMediator : EventMediator
  {
    [Inject]
    public CityDetailsPanelView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(CityDetailsPanelEvent.ChangeArmingCount, OnChangeArmingCount);
      view.dispatcher.AddListener(CityDetailsPanelEvent.CloseArmingPanel, OnCloseArmingPanel);
      view.dispatcher.AddListener(CityDetailsPanelEvent.OnConfirmArming, OnConfirmArming);
      view.dispatcher.AddListener(CityDetailsPanelEvent.OnDoOperation, OnDoOperation);
      view.dispatcher.AddListener(CityDetailsPanelEvent.ClosePanel, OnClose);

      dispatcher.AddListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
      dispatcher.AddListener(MainGameEvent.GameStateChanged, UpdatePanel);
      dispatcher.AddListener(MainGameEvent.PlayerActionsChanged, UpdatePanel);

      Init();
    }

    private void Init()
    {
      view.operationButton.gameObject.SetActive(true);
      view.armingPanel.SetActive(false);
      ArmingPanelStartSettings();

      UpdatePanel();
    }

    private void Update()
    {
      if (view.closing) return;
      
      if (!Input.GetMouseButtonDown(0)) return;

      if (EventSystem.current.IsPointerOverGameObject()) return;

      view.closing = true;
      OnClose();
    }

    private void OnDoOperation()
    {
      switch (mainGameModel.gameStateKey)
      {
        case GameStateKey.ClaimCity:
          OnClaimCity();
          break;
        case GameStateKey.Arming:
          OnArmingCity();
          break;
        case GameStateKey.Attack:
          OnAttack();
          break;
        case GameStateKey.Fortify:
          OnFortify();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void UpdatePanel()
    {
      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      if (view == null)
        return;

      //TODO: view.operationButtonText.text = LocalizationManager.Localize()

      if (mainGameModel.gameStateKey == GameStateKey.ClaimCity)
      {
        view.operationButtonText.text = "Claim City";

        view.operationButton.interactable = cityVo.ownerID == 0 && SetActiveButton(PlayerActionKey.ClaimCity);
      }
      else if (mainGameModel.gameStateKey == GameStateKey.Arming)
      {
        view.operationButtonText.text = "Arming";

        view.armingPanel.gameObject.SetActive(false);
        view.operationButton.interactable =
          cityVo.ownerID == mainGameModel.clientVo.id && mainGameModel.playerFeaturesVo.freeSoldierCount > 0 && SetActiveButton(PlayerActionKey.Arming);
      }
      else if (mainGameModel.gameStateKey == GameStateKey.Attack)
      {
        view.operationButtonText.text = "Attack";

        view.operationButton.interactable = cityVo.ownerID == mainGameModel.clientVo.id && SetActiveButton(PlayerActionKey.Attack);
      }
      else if (mainGameModel.gameStateKey == GameStateKey.Fortify)
      {
        view.operationButtonText.text = "Fortify";

        view.operationButton.interactable = cityVo.ownerID == mainGameModel.clientVo.id && SetActiveButton(PlayerActionKey.Fortify);
      }
      
      if (cityVo.ownerID == 0)
      {
        view.ownerNameText.text = LocalizationManager.Localize("MiniCityInfoPanelNeutral");
        view.ownerColorImage.color = Color.black;
        view.soldierCountText.text = cityVo.soldierCount.ToString();
      }
      else
      {
        ClientVo clientVo = lobbyModel.lobbyVo.clients[cityVo.ownerID];

        view.ownerNameText.text = clientVo.userName;
        view.ownerColorImage.color = clientVo.playerColor.ToColor();
        view.soldierCountText.text = cityVo.soldierCount.ToString();
      }
    }

    private void OnClaimCity()
    {
      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      dispatcher.Dispatch(MainGameEvent.ClaimCity, cityVo);
    }

    private void OnArmingCity()
    {
      PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;

      if (playerFeaturesVo.freeSoldierCount <= 0 && mainGameModel.cities[mainGameModel.selectedCityId].ownerID != mainGameModel.clientVo.id)
        return;

      view.armingPanel.SetActive(true);
      ArmingPanelStartSettings();
    }

    private void ArmingPanelStartSettings()
    {
      view.decreaseButton.interactable = true;
      view.increaseButton.interactable = true;
      view.armingCount = 1;
      view.armingCountText.text = view.armingCount.ToString();
    }

    private void OnChangeArmingCount(IEvent payload)
    {
      PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;

      bool value = (bool)payload.data;

      if (value)
      {
        view.armingCount++;

        if (view.armingCount > playerFeaturesVo.freeSoldierCount)
        {
          view.armingCount = 1;
        }

        view.armingCountText.text = view.armingCount.ToString();
      }
      else
      {
        view.armingCount--;

        if (view.armingCount < 1)
        {
          view.armingCount = playerFeaturesVo.freeSoldierCount;
        }

        view.armingCountText.text = view.armingCount.ToString();
      }
    }

    private void OnConfirmArming()
    {
      view.armingPanel.SetActive(false);
      
      PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;

      if (playerFeaturesVo.freeSoldierCount <= 0)
        return;

      if (view.armingCount > playerFeaturesVo.freeSoldierCount)
        view.armingCount = playerFeaturesVo.freeSoldierCount;

      if (view.armingCount < 1)
        view.armingCount = 1;

      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      ArmingVo armingVo = new()
      {
        cityID = cityVo.ID,
        soldierCount = view.armingCount
      };

      dispatcher.Dispatch(MainGameEvent.ArmingToCity, armingVo);
    }

    private void OnCloseArmingPanel()
    {
      view.armingPanel.SetActive(false);
      view.operationButton.gameObject.SetActive(true);
    }

    private void OnAttack()
    {
      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      OnClose();

      dispatcher.Dispatch(MainGameEvent.SelectCityToAttack, cityVo);
    }

    private void OnFortify()
    {
      CityVo city = mainGameModel.cities[mainGameModel.selectedCityId];
      List<int> neighbors = city.neighbors.Where(t => mainGameModel.cities[t].ownerID == city.ownerID).ToList();

      for (int i = 0; i < neighbors.Count; i++)
      {
        CityVo vo = mainGameModel.cities[neighbors[i]];
        for (int j = 0; j < vo.neighbors.Count; j++)
        {
          if (!neighbors.Contains(vo.neighbors[j]) && vo.neighbors[j] != city.ID && mainGameModel.cities[vo.neighbors[j]].ownerID == city.ownerID)
          {
            neighbors.Add(vo.neighbors[j]);
          }
        }
      }

      KeyValuePair<int, List<int>> cityAndNeighbors = new(city.ID, neighbors);

      dispatcher.Dispatch(MainGameEvent.Fortify, cityAndNeighbors);
      
      OnClose();
    }

    private bool SetActiveButton(PlayerActionKey playerActionKey)
    {
      if (mainGameModel.actionsReferenceList.Count == 0)
        return false;

      PlayerActionPermissionReferenceVo vo = mainGameModel.actionsReferenceList[playerActionKey];

      if (!vo.gameStateKeys.Contains(mainGameModel.gameStateKey))
        return false;

      for (int i = 0; i < vo.playerActionNecessaryKeys.Count; i++)
      {
        if (mainGameModel.playerActionKey.Contains(vo.playerActionNecessaryKeys.ElementAt(i))) continue;
        return false;
      }

      return true;
    }

    private void OnClose()
    {
      screenManagerModel.CloseSpecificPanel(MainGameKeys.CityDetailsPanel);
    }

    public override void OnRemove()
    {
      dispatcher.Dispatch(MainGameEvent.CityDetailsPanelClosed);

      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ChangeArmingCount, OnChangeArmingCount);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.CloseArmingPanel, OnCloseArmingPanel);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.OnConfirmArming, OnConfirmArming);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.OnDoOperation, OnDoOperation);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ClosePanel, OnClose);

      dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
      dispatcher.RemoveListener(MainGameEvent.GameStateChanged, UpdatePanel);
      dispatcher.RemoveListener(MainGameEvent.PlayerActionsChanged, UpdatePanel);
    }
  }
}