using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.View.MiniGameStatsPanel.Item;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.MainGame.View.MiniGameStatsPanel
{
  public class MiniGameResultPanelMediator : EventMediator
  {
    [Inject]
    public MiniGameResultPanelView view { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      SetItems();
      StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
      yield return new WaitForSeconds(PanelClosingTimes.miniGameResults);
      
      dispatcher.Dispatch(MainGameEvent.ShowHideMiniBottomPanel, true);

      screenManagerModel.CloseSpecificPanel(MainGameKeys.MiniGameResultPanel);
    }
    public void SetItems()
    {
      List<MiniGameResultVo> vos = mainGameModel.miniGameResultVos;

      for (int i = 0; i < vos.Count; i++)
      {
        GameObject miniGameStatsItem = Instantiate(view.item, view.container);
        MiniGameResultPanelItemView behaviour = miniGameStatsItem.GetComponent<MiniGameResultPanelItemView>();
        MiniGameResultVo miniGameResultVo = vos.ElementAt(i);
        
        behaviour.SetItem(miniGameResultVo);
      }
    }

    public override void OnRemove()
    {
    }
  }
}