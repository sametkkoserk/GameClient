using System;
using System.Collections.Generic;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.ColorPalette.Enum;
using Runtime.Modules.Core.PromiseTool;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel
{
  public class ColorPaletteModel : IColorPaletteModel
  {
    [Inject]
    public IBundleModel bundleModel { get; set; }
    
    public static IColorPaletteModel instance { get; set; }
    
    protected ColorPaletteKey colorPaletteKey { get; set; }
    
    protected List<ColorPaletteVo> colorPaletteVos { get; set; }
    
    
    public delegate void ColorPaletteChangedEventHandler();
    
    
    public event ColorPaletteChangedEventHandler OnColorPaletteChangedTrigger;

    [PostConstruct]
    public void OnPostConstruct()
    {
      colorPaletteVos = new List<ColorPaletteVo>();
    }

    public IPromise Init()
    {
      Promise promise = new();

      bundleModel.LoadAssetAsync<ColorPaletteData>("ColorPaletteSettings").Then(data =>
      {
        foreach (ColorPaletteVo colorPaletteVo in data.colorPaletteVos)
          colorPaletteVos.Add(colorPaletteVo);

        instance = this;

        promise.Resolve();
      }).Catch(promise.Reject);

      return promise;
    }

    public ColorPaletteKey GetColorPalette()
    {
      return colorPaletteKey;
    }

    public Color ChangeColor(ColorKey colorKey)
    {
      for (int i = 0; i < colorPaletteVos.Count; i++)
      {
        if (colorPaletteVos[i].colorPaletteKey != colorPaletteKey) continue;
        for (int j = 0; j < colorPaletteVos[i].colorVos.Count; j++)
        {
          if (colorPaletteVos[i].colorVos[j].colorKey != colorKey) continue;
          return colorPaletteVos[i].colorVos[j].color;
        }
      }

      return Color.white;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ChangeColorPalette(ColorPaletteKey newColorPaletteKey)
    {
      colorPaletteKey = newColorPaletteKey;
      
      OnColorPaletteChangedTrigger?.Invoke();
    }
    
    public void BindMethod(Action callback)
    {
      ColorPaletteChangedEventHandler handler = () => callback();

      OnColorPaletteChangedTrigger += handler;
      // OnColorPaletteChangedTrigger += callback;
    }
  }
}