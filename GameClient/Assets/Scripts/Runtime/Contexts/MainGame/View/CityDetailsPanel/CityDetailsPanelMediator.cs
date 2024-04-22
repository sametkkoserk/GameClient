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
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public enum CityDetailsPanelEvent
  {
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
      view.dispatcher.AddListener(CityDetailsPanelEvent.OnDoOperation, OnDoOperation);
      view.dispatcher.AddListener(CityDetailsPanelEvent.ClosePanel, OnClose);

      dispatcher.AddListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
      dispatcher.AddListener(MainGameEvent.GameStateChanged, UpdatePanel);

      Init();
    }

    private void Init()
    {
      view.operationButton.gameObject.SetActive(true);

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
        if (IsCityNeutral(cityVo.ownerID) && IsTurnMine())
        {
          view.operationButtonText.text = "Claim City";

          view.operationButton.interactable = true;
        }
        else if (IsCityMine(cityVo.ownerID) && IsTurnMine())
        {
          view.operationButtonText.text = "Support";
          
          view.operationButton.interactable = true;
        }
        else
        {
          view.operationButtonText.text = "Claim City";
          
          view.operationButton.interactable = false;
        }
      }
      else if (mainGameModel.gameStateKey == GameStateKey.Arming)
      {
        view.operationButtonText.text = "Arming";

        view.operationButton.interactable =
          cityVo.ownerID == mainGameModel.clientVo.id && mainGameModel.playerFeaturesVo.freeSoldierCount > 0 && IsTurnMine();
      }
      else if (mainGameModel.gameStateKey == GameStateKey.Attack)
      {
        view.operationButtonText.text = "Attack";

        view.operationButton.interactable = cityVo.ownerID == mainGameModel.clientVo.id && IsTurnMine();
      }
      else if (mainGameModel.gameStateKey == GameStateKey.Fortify)
      {
        view.operationButtonText.text = "Fortify";

        view.operationButton.interactable = cityVo.ownerID == mainGameModel.clientVo.id && IsTurnMine();
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
      if (!IsTurnMine()) return;
      
      PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;
      if (playerFeaturesVo.freeSoldierCount <= 0) return;
      
      ClaimCityVo claimCityVo = new()
      {
        cityId = mainGameModel.selectedCityId,
        soldierCount = 1
      };
        
      dispatcher.Dispatch(MainGameEvent.ClaimCity, claimCityVo);
    }

    private void OnArmingCity()
    {
      dispatcher.Dispatch(MainGameEvent.ShowSelectorPartInBottomPanel);
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

    private bool IsTurnMine()
    {
      return mainGameModel.queueID == lobbyModel.clientVo.id;
    }

    private bool IsCityMine(int ownerId)
    {
      return ownerId == lobbyModel.clientVo.id;
    }

    private bool IsCityNeutral(int ownerId)
    {
      return ownerId == 0;
    }

    private void OnClose()
    {
      screenManagerModel.CloseSpecificPanel(MainGameKeys.CityDetailsPanel);
    }

    public override void OnRemove()
    {
      dispatcher.Dispatch(MainGameEvent.CityDetailsPanelClosed);

      view.dispatcher.RemoveListener(CityDetailsPanelEvent.OnDoOperation, OnDoOperation);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ClosePanel, OnClose);

      dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
      dispatcher.RemoveListener(MainGameEvent.GameStateChanged, UpdatePanel);
    }
  }
}