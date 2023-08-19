using Runtime.Modules.Core.Icon.View;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.MainGame.View.MainHudPanel
{
  public class MainHudPanelView : EventView
  {
    [Header("Time")]
    public Image sliderImage;

    public TextMeshProUGUI timer;

    [Header("Player List")]
    public GameObject playerListItem;

    public Transform playerListContainer;

    public IconView showHidePlayerListButtonIcon;

    [HideInInspector]
    public float totalTime;

    [HideInInspector]
    public bool showPlayerList = true;

    public void OnChangePlayerListSize()
    {
      dispatcher.Dispatch(MainHudPanelEvent.ChangeSizeOfPlayerList, showPlayerList);
    }
  }
}