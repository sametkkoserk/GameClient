using Runtime.Modules.Core.Icon.Enum;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Icon.View
{
  [RequireComponent(typeof(Image))]
  public class IconBehaviour : MonoBehaviour
  {
    public Color color = Color.white;
    
    [Space]
    public IconKey iconKey;

    private Image image;

    private SpriteAtlas spriteAtlas;
    private void OnEnable()
    {
      //Load a text file (Assets/Resources/Icon/IconKey.)
      image = gameObject.GetComponent<Image>();
      spriteAtlas = Resources.Load<SpriteAtlas>("Icon/IconSpriteAtlas");
      
      Init(iconKey, color);
    }

    public void Init(IconKey newIconKey, Color? newColor = null)
    {
      image.color = newColor ?? Color.white;
      iconKey = newIconKey;
      image.sprite = spriteAtlas.GetSprite(iconKey.ToString());
    }
#if UNITY_EDITOR
    public void SetFromInspector()
    {
      image = gameObject.GetComponent<Image>();
      spriteAtlas = Resources.Load<SpriteAtlas>("Icon/IconSpriteAtlas");
      
      image.color = color;
      image.sprite = spriteAtlas.GetSprite(iconKey.ToString());
    }
#endif
  }
}