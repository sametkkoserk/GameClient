using Runtime.Modules.Core.Icon.Enum;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Icon.View
{
  [RequireComponent(typeof(Image))]
  public class IconView : EventView
  {
    public Color color = Color.white;
    
    [Space]
    public IconKey iconKey;

    [HideInInspector]
    public Image image;

    
#if UNITY_EDITOR
    public void SetFromInspector()
    {
      image = gameObject.GetComponent<Image>();
      SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("Icon/IconSpriteAtlas");
      
      image.color = color;
      image.sprite = spriteAtlas.GetSprite(iconKey.ToString());
    }
#endif
  }
}