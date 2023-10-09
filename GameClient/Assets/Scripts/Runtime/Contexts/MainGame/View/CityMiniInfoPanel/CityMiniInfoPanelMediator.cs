using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
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

      if (cityVo.ownerID == 0)
      {
        view.ownerNameText.text = LocalizationManager.Localize("MiniCityInfoPanelNeutral");
        view.ownerColorImage.color = Color.black;
        view.itemOneText.text = cityVo.soldierCount.ToString();
      }
      else
      {
        ClientVo clientVo = lobbyModel.lobbyVo.clients[cityVo.ownerID];

        view.ownerNameText.text = clientVo.userName;
        view.ownerColorImage.color = clientVo.playerColor.ToColor();
        view.itemOneText.text = cityVo.soldierCount.ToString();
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