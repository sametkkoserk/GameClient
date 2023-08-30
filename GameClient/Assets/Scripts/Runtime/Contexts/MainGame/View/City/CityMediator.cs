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
      
      transform.DOMoveY(0f, 0.5f); // Tikladiginda secili oldugunu gosteren
                                   // shader vs. bir sey yapilacak ve boyutu kuculmeyecek. Panel kapandiginda kuculur.
      CursorModel.instance.OnChangeCursor(CursorKey.Default);

      mainGameModel.selectedCityId = view.cityVo.ID;
      screenManagerModel.OpenPanel(MainGameKeys.CityDetailsPanel, SceneKey.MainGame, LayerKey.FirstLayer, PanelMode.Additive, PanelType.LeftPanel);
    }

    private void OnPointerEnter()
    {
      if (!CheckingRequirementForClaim())
        return;
      
      transform.DOMoveY(0.25f, 0.5f);
      CursorModel.instance.OnChangeCursor(CursorKey.Click);

      dispatcher.Dispatch(MainGameEvent.ShowCityMiniInfoPanel, view.cityVo);
    }

    private void OnPointerExit()
    {
      if (!CheckingRequirementForClaim())
        return;
      
      transform.DOMoveY(0f, 0.5f);
      CursorModel.instance.OnChangeCursor(CursorKey.Default);

      dispatcher.Dispatch(MainGameEvent.HideCityMiniInfoPanel);
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
    }
  }
}