using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.TooltipManager
{
  public enum TooltipManagerEvent
  {
    OnPointerEnter,
    OnPointerExit
  }
  public class TooltipManagerMediator : EventMediator
  {
    [Inject]
    public TooltipManagerView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(TooltipManagerEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.AddListener(TooltipManagerEvent.OnPointerExit, OnPointerExit);
    }
    private void OnPointerEnter()
    {
      
    }
    
    private void OnPointerExit()
    {
      throw new System.NotImplementedException();
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(TooltipManagerEvent.OnPointerEnter, OnPointerEnter);
      view.dispatcher.RemoveListener(TooltipManagerEvent.OnPointerEnter, OnPointerExit);
    }
  }
}