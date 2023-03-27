using System;
using Runtime.Modules.Core.Icon.Enum;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Icon.View
{   
  [RequireComponent(typeof(Image))]
  public class IconBehaviour : MonoBehaviour
  {
    [Space]
    public IconKey IconKey;

    private SpriteAtlas spriteAtlas;

    private Image image;
    private void OnEnable()
    {
      //Load a text file (Assets/Resources/Icon/IconKey.)
      image = gameObject.GetComponent<Image>();
      spriteAtlas = Resources.Load<SpriteAtlas>("Icon/IconSpriteAtlas");

      image.sprite = spriteAtlas.GetSprite(IconKey.ToString());
    }

    public void Init(IconKey iconKey)
    {
      IconKey = iconKey;
      image.sprite = spriteAtlas.GetSprite(IconKey.ToString());
    }
#if UNITY_EDITOR
    public void SetFromInspector()
    {
      image = gameObject.GetComponent<Image>();
      spriteAtlas = Resources.Load<SpriteAtlas>("Icon/IconSpriteAtlas");
      
      image.sprite = spriteAtlas.GetSprite(IconKey.ToString());
    }
#endif
  }
}