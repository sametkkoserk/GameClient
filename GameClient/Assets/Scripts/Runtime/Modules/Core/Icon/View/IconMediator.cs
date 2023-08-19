using Runtime.Modules.Core.Icon.Enum;
using Runtime.Modules.Core.Icon.Model;
using Runtime.Modules.Core.Icon.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Modules.Core.Icon.View
{
  public enum IconEvent
  {
    ChangeIcon
  }
  public class IconMediator : EventMediator
  {
    [Inject]
    public IconView view { get; set; }
    
    [Inject]
    public IIconModel iconModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(IconEvent.ChangeIcon, ChangeIcon);
      
      view.image = gameObject.GetComponent<Image>();

      Init(view.iconKey, view.color);
    }

    public void Init(IconKey newIconKey, Color? newColor = null)
    {
      view.image.color = newColor ?? Color.white;
      view.iconKey = newIconKey;
      view.image.sprite = iconModel.spriteAtlas.GetSprite(view.iconKey.ToString());
    }

    public void ChangeIcon(IEvent payload)
    {
      IconVo iconVo = (IconVo)payload.data;
      
      Init(iconVo.iconKey, iconVo.color);
    }
    
    public override void OnRemove()
    {
    }
  }
}