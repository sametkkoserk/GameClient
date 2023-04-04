using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Modules.Core.ColorPalette.Enum
{
  [Serializable]
  public class ColorPaletteVo
  {
    [Space]
    [HideInInspector]
    public string name;
    
    public ColorPaletteKey colorPaletteKey;

    public List<ColorVo> colorVos;
  }
}