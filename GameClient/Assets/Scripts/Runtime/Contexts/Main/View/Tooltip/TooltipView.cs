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
  }
}