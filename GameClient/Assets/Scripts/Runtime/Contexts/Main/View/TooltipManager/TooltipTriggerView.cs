using Runtime.Modules.Core.Localization.Enum;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine.EventSystems;

namespace Runtime.Contexts.Main.View.TooltipManager
{
  public class TooltipTriggerView : EventView, IPointerEnterHandler, IPointerExitHandler
  {
    public TranslateKeys headerTranslateKey;

    public TranslateKeys contentTranslateKey;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
      dispatcher.Dispatch(TooltipTriggerEvent.OnPointerEnter);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
      dispatcher.Dispatch(TooltipTriggerEvent.OnPointerExit);
    }

    private void OnDisable()
    {
      dispatcher.Dispatch(TooltipTriggerEvent.OnDisable);
    }
  }
}