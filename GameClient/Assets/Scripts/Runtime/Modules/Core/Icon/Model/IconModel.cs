using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using UnityEngine.U2D;

namespace Runtime.Modules.Core.Icon.Model
{
  public class IconModel : IIconModel
  {
    public SpriteAtlas spriteAtlas { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      spriteAtlas = Resources.Load<SpriteAtlas>("Icon/IconSpriteAtlas");
    }
  }
}