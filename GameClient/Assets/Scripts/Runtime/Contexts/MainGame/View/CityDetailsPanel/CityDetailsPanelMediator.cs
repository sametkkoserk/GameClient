using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public enum CityDetailsPanelEvent
  {
    ClosePanel,
    ClaimCity,
    Arming,
    ChangeArmingCount,
    CloseArmingPanel,
    OnConfirmArming
  }

  public class CityDetailsPanelMediator : EventMediator
  {
    [Inject]
    public CityDetailsPanelView view { get; set; }

    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(CityDetailsPanelEvent.ClosePanel, OnClose);
      view.dispatcher.AddListener(CityDetailsPanelEvent.ClaimCity, OnClaimCity);
      view.dispatcher.AddListener(CityDetailsPanelEvent.Arming, OnArmingCity);
      view.dispatcher.AddListener(CityDetailsPanelEvent.ChangeArmingCount, OnChangeArmingCount);
      view.dispatcher.AddListener(CityDetailsPanelEvent.CloseArmingPanel, OnCloseArmingPanel);
      view.dispatcher.AddListener(CityDetailsPanelEvent.OnConfirmArming, OnConfirmArming);

      dispatcher.AddListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
      dispatcher.AddListener(MainGameEvent.GameStateChanged, UpdatePanel);
      dispatcher.AddListener(MainGameEvent.PlayerActionsChanged, UpdatePanel);

      Init();
    }

    private void Init()
    {
      view.armingPanel.SetActive(false);
      ArmingPanelStartSettings();

      UpdatePanel();
    }

    public void UpdatePanel()
    {
      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      if (view == null)
        return;

      view.claimButton.interactable = cityVo.ownerID == 0 && SetActiveButton(PlayerActionKey.ClaimCity);
      view.claimButton.gameObject.SetActive(mainGameModel.gameStateKey == GameStateKey.ClaimCity);
      
      view.armingButton.interactable = cityVo.ownerID == mainGameModel.clientVo.id && mainGameModel.playerFeaturesVo.freeSoldierCount > 0 &&
                                       SetActiveButton(PlayerActionKey.Arming);
      view.armingButton.gameObject.SetActive(mainGameModel.gameStateKey == GameStateKey.Arming);
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
      view.buttons.SetActive(false);
      ArmingPanelStartSettings();
    }

    private void ArmingPanelStartSettings()
    {
      view.decreaseButton.interactable = false;
      view.armingCount = 1;
      view.armingCountText.text = view.armingCount.ToString();

      if (view.armingCount >= mainGameModel.playerFeaturesVo.freeSoldierCount)
      {
        view.increaseButton.interactable = false;
      }
    }

    private void OnChangeArmingCount(IEvent payload)
    {
      PlayerFeaturesVo playerFeaturesVo = mainGameModel.playerFeaturesVo;

      bool value = (bool)payload.data;

      view.decreaseButton.interactable = true;
      view.increaseButton.interactable = true;

      if (value)
      {
        view.armingCount++;

        if (view.armingCount >= playerFeaturesVo.freeSoldierCount)
        {
          view.increaseButton.interactable = false;
          view.armingCount = playerFeaturesVo.freeSoldierCount;
        }

        view.armingCountText.text = view.armingCount.ToString();
      }
      else
      {
        view.armingCount--;

        if (view.armingCount <= 1)
        {
          view.decreaseButton.interactable = false;
          view.armingCount = 1;
        }

        view.armingCountText.text = view.armingCount.ToString();
      }
    }
    
    private void OnConfirmArming()
    {
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
        cityVo = cityVo,
        soldierCount = view.armingCount
      };

      dispatcher.Dispatch(MainGameEvent.ArmingToCity, armingVo);
    }

    private void OnCloseArmingPanel()
    {
      view.armingPanel.SetActive(false);
      view.buttons.SetActive(true);
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

      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ClosePanel, OnClose);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ClaimCity, OnClaimCity);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.Arming, OnArmingCity);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ChangeArmingCount, OnChangeArmingCount);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.CloseArmingPanel, OnCloseArmingPanel);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.OnConfirmArming, OnConfirmArming);

      dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
      dispatcher.RemoveListener(MainGameEvent.GameStateChanged, UpdatePanel);
      dispatcher.RemoveListener(MainGameEvent.PlayerActionsChanged, UpdatePanel);
    }
  }
}