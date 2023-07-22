using DG.Tweening;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.Main.View.Tooltip
{
  public class TooltipView : EventView
  {
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public RectTransform rectTransform;

    public Image background;
    
    public void FadeAnimation(float time, float fadeValue)
    {
      headerField.DOColor(new Color(headerField.color.r, headerField.color.g, headerField.color.b, fadeValue), time);
      contentField.DOColor(new Color(contentField.color.r, contentField.color.g, contentField.color.b, fadeValue), time);
      background.DOFade(fadeValue, time);
    }
  }
}