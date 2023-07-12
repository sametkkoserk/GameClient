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

    public float v1 = -0.15f;
    public float v2 = -0.1f;
    public float v3 = 1.1f;
    public float v4 = 1.3f;
  }
}