using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.CityDetailsPanel
{
  public enum CityDetailsPanelEvent
  {
    ClosePanel,
    ClaimCity
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
      
      dispatcher.AddListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);

      UpdatePanel();
    }

    public void UpdatePanel()
    {
      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];
      
      view.claimButton.SetActive(cityVo.ownerID == 0);
    }

    private void OnClaimCity()
    {
      CityVo cityVo = mainGameModel.cities[mainGameModel.selectedCityId];

      dispatcher.Dispatch(MainGameEvent.ClaimCity, cityVo);
    }

    private void OnClose()
    {
      screenManagerModel.CloseSpecificPanel(MainGameKeys.CityDetailsPanel);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ClosePanel, OnClose);
      view.dispatcher.RemoveListener(CityDetailsPanelEvent.ClaimCity, OnClaimCity);
      
      dispatcher.RemoveListener(MainGameEvent.UpdateDetailsPanel, UpdatePanel);
    }
  }
}