using System;
using UnityEngine;

namespace Runtime.Modules.Core.ColorPalette.Enum
{
  [Serializable]
  public class ColorVo
  {
    [HideInInspector]
    public string name;
    
    public ColorKey colorKey;

    [SerializeField]
    public Color color = new Color(1,1,1,1);
  }
}