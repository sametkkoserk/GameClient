using Editor.Tools.DebugX.Runtime;
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
    OnPointerExit,
    OnDisable
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
      view.dispatcher.AddListener(TooltipTriggerEvent.OnDisable, OnDisable);
    }

    private void OnPointerEnter()
    {
      TooltipInfoVo vo = new()
      {
        position = transform.position,
        headerKey = view.headerTranslateKey,
        contentKey = view.contentTranslateKey
      };
      
      crossDispatcher.Dispatch(TooltipEvent.Show, vo);
      DebugX.Log(DebugKey.Tooltip, "Tooltip Trigger Mediator OnPointerEnter");
    }
    
    private void OnPointerExit()
    {
      crossDispatcher.Dispatch(TooltipEvent.Hide, 1f);
      DebugX.Log(DebugKey.Tooltip, "Tooltip Trigger Mediator OnPointerExit");
    }
    
    private void OnDisable()
    {
      crossDispatcher.Dispatch(TooltipEvent.Hide, 0f);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(TooltipTriggerEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(TooltipTriggerEvent.OnPointerEnter, OnPointerExit);
      view.dispatcher.RemoveListener(TooltipTriggerEvent.OnDisable, OnDisable);
      
      DebugX.Log(DebugKey.Tooltip, "Tooltip Trigger Mediator OnRemove");
    }
  }
}