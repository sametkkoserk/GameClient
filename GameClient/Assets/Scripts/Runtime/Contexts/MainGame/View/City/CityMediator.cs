using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.City
{
  public enum CityEvent
  {
    OnClick,
    OnPointerEnter,
    OnPointerExit,
    OnUpdateCity,
  }

  public class CityMediator : EventMediator
  {
    [Inject]
    public CityView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(CityEvent.OnClick, OnClick);
      view.dispatcher.AddListener(CityEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.AddListener(CityEvent.OnPointerExit, OnPointerExit);
      view.dispatcher.AddListener(CityEvent.OnUpdateCity, OnUpdateCity);

      dispatcher.AddListener(MainGameEvent.PlayerActionsReferenceListExecuted, OnPlayerActionsReferenceListExecuted);
      dispatcher.AddListener(MainGameEvent.ClaimedCity, OnClaimedCity);
      dispatcher.AddListener(MainGameEvent.SelectCityToAttack, OnSelectCityToAttack);
      dispatcher.AddListener(MainGameEvent.CityDetailsPanelClosed, CityDetailsClosed);
      dispatcher.AddListener(MainGameEvent.UpdateDetailsPanel, OnUpdateCity);
      dispatcher.AddListener(MainGameEvent.GameStateChanged, OnUpdateCity);
      dispatcher.AddListener(MainGameEvent.PlayerActionsChanged, OnUpdateCity);
      dispatcher.AddListener(MainGameEvent.ResetCityMode, OnResetCityMode);
      dispatcher.AddListener(MainGameEvent.AttackResult, OnAttackResult);
      dispatcher.AddListener(MainGameEvent.Fortify, OnFortify);
      dispatcher.AddListener(MainGameEvent.FortifyResult, OnFortifyResult);
    }

    private void OnPlayerActionsReferenceListExecuted()
    {
      PlayerActionPermissionReferenceVo vo = mainGameModel.actionsReferenceList[view.playerActionKey];

      view.necessaryPlayerActionKeysForOpenDetailsPanel = vo.playerActionNecessaryKeys;
      view.necessaryGameStateKeyForOpenDetailsPanel = vo.gameStateKeys;
    }

    private void OnUpdateCity()
    {
      if (!view.cityVo.isPlayable)
      {
        view.playableCityObjects.SetActive(false);
        dispatcher.RemoveListener(CityEvent.OnUpdateCity, OnUpdateCity);
        return;
      }
      view.soldierCountText.text = view.cityVo.soldierCount.ToString();

      if (mainGameModel.selectedCityId == view.cityVo.ID)
        return;
      
      Highlight(0);
      // soldierCountText.colorGradientPreset.bottomLeft = Color.black;
    }

    public void OnClick()
    {
      if (!CheckingRequirementToClick())
        return;

      if (view.GetCityModeKey() == CityModeKey.Attacker)
      {
        dispatcher.Dispatch(MainGameEvent.ResetCityMode);
      }
      else if (view.GetCityModeKey() == CityModeKey.AttackTarget)
      {
        AttackVo attackVo = new()
        {
          attackerCityVo = mainGameModel.cities[mainGameModel.selectedCityId],
          defenderCityVo = view.cityVo
        };
        
        dispatcher.Dispatch(MainGameEvent.ConfirmAttack, attackVo);
        dispatcher.Dispatch(MainGameEvent.ResetCityMode);
      }
      else if (view.GetCityModeKey() == CityModeKey.FortifySource)
      {
        dispatcher.Dispatch(MainGameEvent.ResetCityMode);
      }
      else if (view.GetCityModeKey() == CityModeKey.FortifyTarget)
      {
        FortifyVo fortifyVo = new()
        {
          sourceCityId = mainGameModel.selectedCityId,
          targetCityId = view.cityVo.ID
        };
        
        dispatcher.Dispatch(MainGameEvent.ConfirmFortify, fortifyVo);
        dispatcher.Dispatch(MainGameEvent.ResetCityMode);
      }
      else if (view.GetCityModeKey() == CityModeKey.None)
      {
        if (view.cityVo.ID == mainGameModel.selectedCityId)
        {
          Highlight(0);
          mainGameModel.selectedCityId = -1;
          return;
        }
        
        Highlight(0.125f, CursorKey.Click);
        mainGameModel.selectedCityId = view.cityVo.ID;
        screenManagerModel.OpenPanel(MainGameKeys.CityDetailsPanel, SceneKey.MainGame, LayerKey.ThirdLayer, PanelMode.Destroy, PanelType.LeftPanel);
      }
    }

    private IEnumerator WaitClosingPanel()
    {
      yield return new WaitForSeconds(0.2f);
      
      Highlight(0.125f, CursorKey.Click);
    }

    private void OnPointerEnter()
    {
      if (!CheckingRequirementToClick())
        return;

      if (view.GetCityModeKey() == CityModeKey.Attacker || view.GetCityModeKey() == CityModeKey.AttackTarget ||
          view.GetCityModeKey() == CityModeKey.FortifySource || view.GetCityModeKey() == CityModeKey.FortifyTarget)
      {
        CursorModel.instance.OnChangeCursor(CursorKey.Click);

        return;
      }
      
      Highlight(0.125f, CursorKey.Click);

      // dispatcher.Dispatch(MainGameEvent.ShowCityMiniInfoPanel, view.cityVo);
    }

    private void OnPointerExit()
    {
      if (!CheckingRequirementToClick())
        return;
      
      // dispatcher.Dispatch(MainGameEvent.HideCityMiniInfoPanel);
      CursorModel.instance.OnChangeCursor(CursorKey.Default);

      if (mainGameModel.selectedCityId == view.cityVo.ID)
        return;

      if (view.GetCityModeKey() == CityModeKey.Attacker || view.GetCityModeKey() == CityModeKey.AttackTarget ||
          view.GetCityModeKey() == CityModeKey.FortifySource || view.GetCityModeKey() == CityModeKey.FortifyTarget)
        return;
      
      Highlight(0f);
    }

    private void Highlight(float endValue, CursorKey cursorKey = CursorKey.Default)
    {
      transform.DOMoveY(endValue, 0.5f);
      CursorModel.instance.OnChangeCursor(cursorKey);
    }

    private void CityDetailsClosed()
    {
      if (mainGameModel.selectedCityId == view.cityVo.ID)
        return;

      if (view.GetCityModeKey() == CityModeKey.None) 
        Highlight(0);
    }

    private bool CheckingRequirementToClick()
    {
      if (!view.GetClickable())
        return false;
      
      if (mainGameModel.actionsReferenceList.Count == 0)
        return false;
      
      if (!view.necessaryGameStateKeyForOpenDetailsPanel.Contains(mainGameModel.gameStateKey))
        return false;

      for (int i = 0; i < view.necessaryPlayerActionKeysForOpenDetailsPanel.Count; i++)
      {
        if (mainGameModel.playerActionKey.Contains(view.necessaryPlayerActionKeysForOpenDetailsPanel.ElementAt(i))) continue;
        return false;
      }

      return true;
    }

    public void OnClaimedCity(IEvent payload)
    {
      CityVo cityVo = (CityVo)payload.data;

      if (cityVo.ID != view.cityVo.ID)
        return;

      view.ChangeOwner(cityVo, lobbyModel.lobbyVo.clients[cityVo.ownerID].playerColor.ToColor());
    }

    private void OnSelectCityToAttack(IEvent payload)
    {
      CityVo cityVo = (CityVo) payload.data;

      view.SetClickable(false);
      view.SetCityModeKey(CityModeKey.None);

      if (!view.cityVo.isPlayable)
        return;

      if (cityVo.ID == view.cityVo.ID)
      {
        view.SetCityModeKey(CityModeKey.Attacker);
        view.SetClickable(true);
        Highlight(0.175f);
        return;
      }
      
      if (cityVo.ownerID == view.cityVo.ownerID)
        return;

      if (!cityVo.neighbors.Contains(view.cityVo.ID))
        return;

      view.SetCityModeKey(CityModeKey.AttackTarget);
      view.SetClickable(true);
      Highlight(0.125f);
    }

    private void OnAttackResult(IEvent payload)
    {
      AttackResultVo attackResultVo = (AttackResultVo)payload.data;

      if (view.cityVo.ID != attackResultVo.winnerCity.ID && view.cityVo.ID != attackResultVo.loserCity.ID)
        return;

      if (attackResultVo.winnerCity.ID == view.cityVo.ID)
      {
        CityVo winnerCity = attackResultVo.winnerCity;

        if (winnerCity.ID != view.cityVo.ID)
          return;

        view.cityVo = mainGameModel.cities[winnerCity.ID];
      }
      else if (attackResultVo.loserCity.ID == view.cityVo.ID)
      {
        CityVo loserCity = attackResultVo.loserCity;

        if (loserCity.ID != view.cityVo.ID)
          return;

        view.ChangeOwner(loserCity, lobbyModel.lobbyVo.clients[loserCity.ownerID].playerColor.ToColor());
      }

      if (attackResultVo.isConquered)
        dispatcher.Dispatch(MainGameEvent.SetTransferSoldierAfterAttack, attackResultVo);
    }

    private void OnFortify(IEvent payload)
    {
      KeyValuePair<int, List<int>> neighbors = (KeyValuePair<int, List<int>>)payload.data;

      view.SetClickable(false);
      view.SetCityModeKey(CityModeKey.None);

      if (neighbors.Key == view.cityVo.ID)
      {
        Highlight(0.175f);
        view.SetClickable(true);
        view.SetCityModeKey(CityModeKey.FortifySource);
        return;
      }

      if (!neighbors.Value.Contains(view.cityVo.ID))
      {
        view.SetClickable(false);
        return;
      }
      
      view.SetClickable(true);
      view.SetCityModeKey(CityModeKey.FortifyTarget);
      Highlight(0.125f);
    }

    private void OnFortifyResult(IEvent payload)
    {
      FortifyResultVo fortifyResultVo = (FortifyResultVo)payload.data;

      if (view.cityVo.ID != fortifyResultVo.sourceCity.ID && view.cityVo.ID != fortifyResultVo.targetCity.ID)
        return;

      if (fortifyResultVo.sourceCity.ID == view.cityVo.ID)
      {
        CityVo sourceCity = fortifyResultVo.sourceCity;

        if (sourceCity.ID != view.cityVo.ID)
          return;
        
        view.cityVo = mainGameModel.cities[sourceCity.ID];
      }
      else if (fortifyResultVo.targetCity.ID == view.cityVo.ID)
      {
        CityVo targetCity = fortifyResultVo.targetCity;

        if (targetCity.ID != view.cityVo.ID)
          return;
        
        view.cityVo = mainGameModel.cities[targetCity.ID];
      }
    }
    
    private void OnResetCityMode()
    {
      view.SetCityModeKey(CityModeKey.None);
      view.SetClickable(true);
      Highlight(0);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CityEvent.OnClick, OnClick);
      view.dispatcher.RemoveListener(CityEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(CityEvent.OnPointerExit, OnPointerExit);
      view.dispatcher.RemoveListener(CityEvent.OnUpdateCity, OnUpdateCity);
      
      dispatcher.RemoveListener(MainGameEvent.PlayerActionsReferenceListExecuted, OnPlayerActionsReferenceListExecuted);
      dispatcher.RemoveListener(MainGameEvent.ClaimedCity, OnClaimedCity);
      dispatcher.RemoveListener(MainGameEvent.SelectCityToAttack, OnSelectCityToAttack);
      dispatcher.RemoveListener(MainGameEvent.CityDetailsPanelClosed, CityDetailsClosed);
      dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, OnUpdateCity);
      dispatcher.RemoveListener(MainGameEvent.GameStateChanged, OnUpdateCity);
      dispatcher.RemoveListener(MainGameEvent.PlayerActionsChanged, OnUpdateCity);
      dispatcher.RemoveListener(MainGameEvent.ResetCityMode, OnResetCityMode);
      dispatcher.RemoveListener(MainGameEvent.AttackResult, OnAttackResult);
      dispatcher.RemoveListener(MainGameEvent.Fortify, OnFortify);
      dispatcher.RemoveListener(MainGameEvent.FortifyResult, OnFortifyResult);
    }
  }
}