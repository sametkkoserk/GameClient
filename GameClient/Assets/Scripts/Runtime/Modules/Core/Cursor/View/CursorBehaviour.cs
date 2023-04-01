using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Cursor.View
{
  public class CursorBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public CursorKey onPointerEnter;
    
    public CursorKey onPointerExit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
      Button button = gameObject.GetComponent<Button>();
      TMP_Dropdown dropdown = gameObject.GetComponent<TMP_Dropdown>();

      if (button != null && button.interactable)
        CursorModel.instance.OnChangeCursor(onPointerEnter);

      if (dropdown != null && dropdown.interactable)
        CursorModel.instance.OnChangeCursor(onPointerEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      CursorModel.instance.OnChangeCursor(onPointerExit);
    }

    public void OnDestroy()
    {
      CursorModel.instance.OnChangeCursor(onPointerExit);
    }
  }
}