using System.Collections.Generic;
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

      dispatcher.AddListener(MainGameEvent.ClaimedCity, OnClaimedCity);
      dispatcher.AddListener(MainGameEvent.SelectCityToAttack, OnSelectCityToAttack);
      dispatcher.AddListener(MainGameEvent.CityDetailsPanelClosed, CityDetailsClosed);
      dispatcher.AddListener(MainGameEvent.UpdateDetailsPanel, OnUpdateCity);
      dispatcher.AddListener(MainGameEvent.GameStateChanged, OnUpdateCity);
      dispatcher.AddListener(MainGameEvent.ResetCityMode, OnResetCityMode);
      dispatcher.AddListener(MainGameEvent.AttackResult, OnAttackResult);
      dispatcher.AddListener(MainGameEvent.Fortify, OnFortify);
      dispatcher.AddListener(MainGameEvent.FortifyResult, OnFortifyResult);
      dispatcher.AddListener(MainGameEvent.OutlineSettings, OnSetOutlines);
    }

    private void Start()
    {
      view.outline.enabled = false;
    }

    private void OnUpdateCity()
    {
      if (!view.cityVo.isPlayable)
      {
        view.playableCityObjects.SetActive(false);
        
        dispatcher.RemoveListener(CityEvent.OnUpdateCity, OnUpdateCity);
        dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, OnUpdateCity);
        dispatcher.RemoveListener(MainGameEvent.GameStateChanged, OnUpdateCity);
        
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
      if (!view.GetClickable())
        return;

      if (view.GetCityMode() == CityView.CityMode.None)
      {
        view.SetCityMode(CityView.CityMode.Selected);
        mainGameModel.selectedCityId = view.cityVo.ID;
        
        screenManagerModel.OpenPanel(MainGameKeys.CityDetailsPanel, SceneKey.MainGame, LayerKey.ThirdLayer, PanelMode.Destroy, PanelType.LeftPanel);
      }
      else if (view.GetCityMode() == CityView.CityMode.Selected && view.cityVo.ID == mainGameModel.selectedCityId)
      {
        view.SetCityMode(CityView.CityMode.None);
        mainGameModel.selectedCityId = -1;
      }
      else if (view.GetCityMode() == CityView.CityMode.Source)
      {
        dispatcher.Dispatch(MainGameEvent.ResetCityMode);
      }
      else if (view.GetCityMode() == CityView.CityMode.Target)
      {
        if (mainGameModel.gameStateKey == GameStateKey.Attack)
        {
          AttackVo attackVo = new()
          {
            attackerCityID = mainGameModel.selectedCityId,
            defenderCityID = view.cityVo.ID
          };
        
          dispatcher.Dispatch(MainGameEvent.ConfirmAttack, attackVo);
          dispatcher.Dispatch(MainGameEvent.ResetCityMode);
        }
        else if (mainGameModel.gameStateKey == GameStateKey.Fortify)
        {
          FortifyVo fortifyVo = new()
          {
            sourceCityId = mainGameModel.selectedCityId,
            targetCityId = view.cityVo.ID
          };
          dispatcher.Dispatch(MainGameEvent.SetTransferSoldierForFortify, fortifyVo);
          dispatcher.Dispatch(MainGameEvent.ResetCityMode);
        }
      }
    }

    private void OnPointerEnter()
    {
      if (!view.GetClickable())
        return;

      dispatcher.Dispatch(MainGameEvent.OutlineSettings, view.cityVo.ID);
      CursorModel.instance.OnChangeCursor(CursorKey.Click);
    }

    private void OnPointerExit()
    {
      if (!view.GetClickable())
        return;
      
      CursorModel.instance.OnChangeCursor(CursorKey.Default);
      view.outline.enabled = false;
    }

    private const float highlightHeightValue = 0.15f;
    private void Highlight(float value = highlightHeightValue)
    {
      transform.DOMoveY(value, 0.5f);
    }

    private void CityDetailsClosed()
    {
      if (mainGameModel.selectedCityId == view.cityVo.ID && view.GetCityMode() == CityView.CityMode.Selected)
      {
        view.SetCityMode(CityView.CityMode.None);
      }
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
      view.SetCityMode(CityView.CityMode.None);
    
      if (!view.cityVo.isPlayable)
        return;
    
      if (cityVo.ID == view.cityVo.ID)
      {
        SourceTargetShortCut(CityView.CityMode.Source, true);
        return;
      }
      
      if (cityVo.ownerID == view.cityVo.ownerID)
        return;
    
      if (!cityVo.neighbors.Contains(view.cityVo.ID))
        return;
    
      SourceTargetShortCut(CityView.CityMode.Target, true);
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
      view.SetCityMode(CityView.CityMode.None);
    
      if (neighbors.Key == view.cityVo.ID)
      {
        SourceTargetShortCut(CityView.CityMode.Source, true);
        return;
      }
    
      if (!neighbors.Value.Contains(view.cityVo.ID))
      {
        view.SetClickable(false);
        return;
      }
      
      SourceTargetShortCut(CityView.CityMode.Target, true);
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

    private void SourceTargetShortCut(CityView.CityMode cityMode, bool isClickable, float highlightValue = highlightHeightValue)
    {
      view.SetCityMode(cityMode);
      view.SetClickable(isClickable);
      Highlight(highlightValue);
      view.SetOutlineColor();
    }
      
    private void OnSetOutlines(IEvent payload)
    {
      int id = (int)payload.data;

      if (id == view.cityVo.ID)
      {
        view.outline.enabled = true;
        return;
      }

      view.outline.enabled = false;
    }
    
    private void OnResetCityMode()
    {
      SourceTargetShortCut(CityView.CityMode.None, true, 0);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CityEvent.OnClick, OnClick);
      view.dispatcher.RemoveListener(CityEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(CityEvent.OnPointerExit, OnPointerExit);
      view.dispatcher.RemoveListener(CityEvent.OnUpdateCity, OnUpdateCity);
      
      dispatcher.RemoveListener(MainGameEvent.ClaimedCity, OnClaimedCity);
      dispatcher.RemoveListener(MainGameEvent.SelectCityToAttack, OnSelectCityToAttack);
      dispatcher.RemoveListener(MainGameEvent.CityDetailsPanelClosed, CityDetailsClosed);
      dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, OnUpdateCity);
      dispatcher.RemoveListener(MainGameEvent.GameStateChanged, OnUpdateCity);
      dispatcher.RemoveListener(MainGameEvent.ResetCityMode, OnResetCityMode);
      dispatcher.RemoveListener(MainGameEvent.AttackResult, OnAttackResult);
      dispatcher.RemoveListener(MainGameEvent.Fortify, OnFortify);
      dispatcher.RemoveListener(MainGameEvent.FortifyResult, OnFortifyResult);
      dispatcher.RemoveListener(MainGameEvent.OutlineSettings, OnSetOutlines);
    }
  }
}