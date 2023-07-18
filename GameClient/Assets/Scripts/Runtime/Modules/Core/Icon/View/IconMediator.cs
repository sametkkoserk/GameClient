using Runtime.Modules.Core.Icon.Enum;
using Runtime.Modules.Core.Icon.Model;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Modules.Core.Icon.View
{
  public class IconMediator : EventMediator
  {
    [Inject]
    public IconView view { get; set; }
    
    [Inject]
    public IIconModel iconModel { get; set; }

    public override void OnRegister()
    {
      Init(view.iconKey, view.color);
    }

    public void Init(IconKey newIconKey, Color? newColor = null)
    {
      view.image.color = newColor ?? Color.white;
      view.iconKey = newIconKey;
      view.image.sprite = iconModel.spriteAtlas.GetSprite(view.iconKey.ToString());
    }
    
    public override void OnRemove()
    {
    }
  }
}