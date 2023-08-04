using DG.Tweening;
using Runtime.Modules.Core.Audio.Enum;
using Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Modules.Core.Cursor.View
{
  public class DropdownCursorBehaviour : MonoBehaviour, ICursorBehaviour
  {
    public CursorKey onPointerEnter;
    
    public CursorKey onPointerExit;

    private TMP_Dropdown dropdown;

    private Vector3 originalScale;

    private const float scaleFactor = 1.15f;

    private void Start()
    {
      dropdown = gameObject.GetComponent<TMP_Dropdown>();
      
      originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!Check())
        return;
        
      CursorModel.instance.OnChangeCursor(onPointerEnter);
      
      dropdown.transform.DOScale(originalScale * scaleFactor, 0.2f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
      if (!Check())
        return;
      
      CursorModel.instance.OnChangeCursor(onPointerExit);
    
      dropdown.transform.DOScale(originalScale, 0.2f);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
      if (!Check())
        return;
      
      CursorModel.instance.OnChangeCursor(dropdown.interactable ? onPointerEnter : onPointerExit);

      AudioModel.instance.PlayUISound(UISoundsKey.Click);
    }
    
    public void OnDestroy()
    {
      CursorModel.instance.OnChangeCursor(onPointerExit);
    }

    public bool Check()
    {
      return dropdown != null && dropdown.interactable;
    }
  }
}