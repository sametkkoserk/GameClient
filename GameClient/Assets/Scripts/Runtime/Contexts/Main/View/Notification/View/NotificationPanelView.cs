using DG.Tweening;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.Contexts.Main.View.Notification.View
{
  public class NotificationPanelView : EventView, IPointerClickHandler
  {
    public TextMeshProUGUI headerText;

    public TextMeshProUGUI contentText;

    public LayoutElement layoutElement;
    
    public Image background;

    public Image line;

    private void Start()
    {
      float screenHeight = Screen.height; 
      float stopPositionY = screenHeight * 0.8f;

      Sequence moveSequence = DOTween.Sequence();

      moveSequence.Append(transform.DOMoveY(stopPositionY, 2f));

      moveSequence.Play();
    }
    
    public void FadeAnimation(float time, float fadeValue, float delayTime)
    {
      DOVirtual.DelayedCall(delayTime, () =>
      {
        headerText.DOColor(new Color(headerText.color.r, headerText.color.g, headerText.color.b, fadeValue), time);
        contentText.DOColor(new Color(contentText.color.r, contentText.color.g, contentText.color.b, fadeValue), time);
        background.DOFade(fadeValue, time);
        line.DOFade(fadeValue, time);
      });
      
      DOVirtual.DelayedCall(delayTime + time, () =>
      {
        Destroy(gameObject);
      });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      dispatcher.Dispatch(NotificationPanelEvent.ClosePanel);
    }
  }
}