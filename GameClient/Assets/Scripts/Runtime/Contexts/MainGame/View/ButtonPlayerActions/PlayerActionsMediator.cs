using System.Linq;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.MainGame.View.ButtonPlayerActions
{
  public class PlayerActionsMediator : EventMediator
  {
    [Inject]
    public PlayerActionsView view { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(MainGameEvent.GameStateChanged, OnCheckButton);
      dispatcher.AddListener(MainGameEvent.PlayerActionsChanged, OnCheckButton);
      
      OnCheckButton();
    }

    private void OnCheckButton()
    {
      if (view == null || view.button == null) return;
      view.button.interactable = SetActiveButton();
    }

    private bool SetActiveButton()
    {
      if (mainGameModel.actionsReferenceList.Count == 0)
        return false;
      
      PlayerActionPermissionReferenceVo vo = mainGameModel.actionsReferenceList[view.playerActionKey];

      if (!vo.gameStateKeys.Contains(mainGameModel.gameStateKey))
        return false; 
      
      for (int i = 0; i < vo.playerActionNecessaryKeys.Count; i++)
      {
        if (mainGameModel.playerActionKey.Contains(vo.playerActionNecessaryKeys.ElementAt(i))) continue;
        return false;
      }

      return true;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(MainGameEvent.GameStateChanged, OnCheckButton);
      dispatcher.RemoveListener(MainGameEvent.PlayerActionsChanged, OnCheckButton);
    }
  }
}