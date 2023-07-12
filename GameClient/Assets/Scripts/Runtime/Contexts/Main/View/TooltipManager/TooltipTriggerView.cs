using Runtime.Modules.Core.Localization.Enum;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine.EventSystems;

namespace Runtime.Contexts.Main.View.TooltipManager
{
  public class TooltipManagerView : EventView, IPointerEnterHandler, IPointerExitHandler
  {
    public TranslateKey headerTranslateKey;

    public TranslateKey contentTranslateKey;

    
    public void OnPointerEnter(PointerEventData eventData)
    {
      dispatcher.Dispatch(TooltipManagerEvent.OnPointerEnter);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
      dispatcher.Dispatch(TooltipManagerEvent.OnPointerExit);
    }
  }
}