using System;
using Runtime.Modules.Core.ColorPalette.Enum;
using Runtime.Modules.Core.PromiseTool;
using UnityEngine;

namespace Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel
{
  public interface IColorPaletteModel
  {
    void BindMethod(Action callback);
    IPromise Init();
    ColorPaletteKey GetColorPalette();
    Color ChangeColor(ColorKey colorKey);
    void ChangeColorPalette(ColorPaletteKey newColorPaletteKey);
  }
}