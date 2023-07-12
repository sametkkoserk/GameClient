using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.TooltipManager
{
  public enum TooltipTriggerEvent
  {
    OnPointerEnter,
    OnPointerExit
  }
  public class TooltipTriggerMediator : EventMediator
  {
    [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
    public IEventDispatcher crossDispatcher { get; set; }
    
    [Inject]
    public TooltipTriggerView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(TooltipTriggerEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.AddListener(TooltipTriggerEvent.OnPointerExit, OnPointerExit);
    }
    private void OnPointerEnter()
    {
      TooltipInfoVo vo = new()
      {
        headerKey = view.headerTranslateKey,
        contentKey = view.contentTranslateKey
      };
      
      crossDispatcher.Dispatch(TooltipEvent.Show, vo);
    }
    
    private void OnPointerExit()
    {
      crossDispatcher.Dispatch(TooltipEvent.Hide);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(TooltipTriggerEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(TooltipTriggerEvent.OnPointerEnter, OnPointerExit);
    }
  }
}