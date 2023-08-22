using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.CityMiniInfoPanel
{
  public class CityMiniInfoPanelMediator : EventMediator
  {
    [Inject]
    public CityMiniInfoPanelView view { get; set; }

    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public ILocalizationModel localizationModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.ShowCityMiniInfoPanel, OnShowCityMiniInfoPanel);
      dispatcher.AddListener(MainGameEvent.HideCityMiniInfoPanel, OnHideCityMiniInfoPanel);

      Init();
    }

    private void Init()
    {
      gameObject.SetActive(false);
    }

    private void OnShowCityMiniInfoPanel(IEvent payload)
    {
      CityVo cityVo = (CityVo)payload.data;

      if (cityVo.ownerID == -1)
      {
        view.ownerNameText.text = localizationModel.GetText(TableKey.MainGame, TranslateKeys.MiniCityInfoPanelNeutral);
        view.ownerColorImage.color = Color.black;
        view.itemOneText.text = cityVo.soldierCount.ToString(); // Change!
      }
      else
      {
        ClientVo clientVo = lobbyModel.lobbyVo.clients[(ushort)cityVo.ownerID];

        view.ownerNameText.text = clientVo.userName;
        view.ownerColorImage.color = clientVo.playerColor.ToColor();
      }

      gameObject.SetActive(true);
    }

    private void OnHideCityMiniInfoPanel()
    {
      gameObject.SetActive(false);
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.ShowCityMiniInfoPanel, OnShowCityMiniInfoPanel);
      dispatcher.RemoveListener(MainGameEvent.HideCityMiniInfoPanel, OnHideCityMiniInfoPanel);
    }
  }
}