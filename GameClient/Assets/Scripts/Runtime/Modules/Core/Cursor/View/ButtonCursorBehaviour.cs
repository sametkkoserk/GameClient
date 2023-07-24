using DG.Tweening;
using Runtime.Modules.Core.Audio.Enum;
using Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Cursor.View
{
  public class ButtonCursorBehaviour : MonoBehaviour, ICursorBehaviour
  {
    public CursorKey onPointerEnter;
    
    public CursorKey onPointerExit;

    private Button button;

    private Vector3 originalScale;

    private const float scaleFactor = 1.15f;

    private void Start()
    {
      button = gameObject.GetComponent<Button>();
      
      originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      if (!Check())
        return;
        
      CursorModel.instance.OnChangeCursor(onPointerEnter);
      
      button.transform.DOScale(originalScale * scaleFactor, 0.2f);
    //   TMP_Dropdown dropdown = gameObject.GetComponent<TMP_Dropdown>();

    //
    //   if (dropdown != null && dropdown.interactable)
    //     CursorModel.instance.OnChangeCursor(onPointerEnter);
    //   
    //
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
      if (!Check())
        return;
      CursorModel.instance.OnChangeCursor(onPointerExit);
    
      button.transform.DOScale(originalScale, 0.2f);
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
      return button != null && button.interactable;
    }
  }
}