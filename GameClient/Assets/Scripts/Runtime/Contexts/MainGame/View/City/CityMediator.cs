using System.Collections;
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
    OnPointerExit
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
      
      dispatcher.AddListener(MainGameEvent.PlayerActionsReferenceListExecuted, OnPlayerActionsReferenceListExecuted);
      dispatcher.AddListener(MainGameEvent.ClaimedCity, OnClaimedCity);
      dispatcher.AddListener(MainGameEvent.CityDetailsPanelClosed, CityDetailsClosed);
    }

    private void OnPlayerActionsReferenceListExecuted()
    {
      PlayerActionPermissionReferenceVo vo = mainGameModel.actionsReferenceList[view.playerActionKey];
      
      view.necessaryPlayerActionKeysForOpenDetailsPanel = vo.playerActionNecessaryKeys;
      view.necessaryGameStateKeyForOpenDetailsPanel = vo.gameStateKeys;
    }

    public void OnClick()
    {
      if (!CheckingRequirementForClaim())
        return;

      if (mainGameModel.selectedCityId == view.cityVo.ID)
      {
        mainGameModel.selectedCityId = -1;
        screenManagerModel.CloseSpecificPanel(MainGameKeys.CityDetailsPanel);
        
        StopAllCoroutines();
        StartCoroutine(WaitClosingPanel());
        return;
      }

      SetSize(0.25f, CursorKey.Click);
      mainGameModel.selectedCityId = view.cityVo.ID;
      screenManagerModel.OpenPanel(MainGameKeys.CityDetailsPanel, SceneKey.MainGame, LayerKey.ThirdLayer, PanelMode.Destroy, PanelType.LeftPanel);
    }

    private IEnumerator WaitClosingPanel()
    {
      yield return new WaitForSeconds(0.2f);
      
      SetSize(0.25f, CursorKey.Click);
    }

    private void OnPointerEnter()
    {
      if (!CheckingRequirementForClaim())
        return;
      
      SetSize(0.25f, CursorKey.Click);

      dispatcher.Dispatch(MainGameEvent.ShowCityMiniInfoPanel, view.cityVo);
    }

    private void OnPointerExit()
    {
      if (!CheckingRequirementForClaim())
        return;
      
      dispatcher.Dispatch(MainGameEvent.HideCityMiniInfoPanel);
      CursorModel.instance.OnChangeCursor(CursorKey.Default);

      if (mainGameModel.selectedCityId == view.cityVo.ID)
        return;
      SetSize(0f);
    }

    private void SetSize(float endValue, CursorKey cursorKey = CursorKey.Default)
    {
      transform.DOMoveY(endValue, 0.5f);
      CursorModel.instance.OnChangeCursor(cursorKey);
    }

    private void CityDetailsClosed()
    {
      if (mainGameModel.selectedCityId == view.cityVo.ID)
        return;
      
      SetSize(0);
    }

    private bool CheckingRequirementForClaim()
    {
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

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CityEvent.OnClick, OnClick);
      view.dispatcher.RemoveListener(CityEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(CityEvent.OnPointerExit, OnPointerExit);
      
      dispatcher.RemoveListener(MainGameEvent.PlayerActionsReferenceListExecuted, OnPlayerActionsReferenceListExecuted);
      dispatcher.RemoveListener(MainGameEvent.ClaimedCity, OnClaimedCity);
      dispatcher.RemoveListener(MainGameEvent.CityDetailsPanelClosed, CityDetailsClosed);
    }
  }
}