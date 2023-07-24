using UnityEngine.EventSystems;

namespace Runtime.Modules.Core.Cursor.View
{
  public interface ICursorBehaviour : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
  {
    new void OnPointerEnter(PointerEventData eventData);
    
    new void OnPointerExit(PointerEventData eventData);
    
    new void OnPointerClick(PointerEventData eventData);
  }
}