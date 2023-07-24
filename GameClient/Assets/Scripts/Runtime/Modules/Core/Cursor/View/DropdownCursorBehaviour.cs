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

    private TMP_InputField dropdown;

    private Vector3 originalScale;

    private const float scaleFactor = 1.1f;
    
    private void Start()
    {
      dropdown = gameObject.GetComponent<TMP_InputField>();
      
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