using System;
using System.Collections.Generic;
using Runtime.Modules.Core.ColorPalette.Enum;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.ColorPalette.Model
{
  [CreateAssetMenu(menuName = "Tools/ColorPalette/Create", fileName = "ColorPaletteSettings")]
  [Serializable]
  public class ColorPaletteData : ScriptableObject
  {
    [SerializeField]
    public List<ColorPaletteVo> colorPaletteVos;

    public static bool init;

    public static AsyncOperationHandle<ColorPaletteData> colorPaletteData;
    
#if UNITY_EDITOR
    public static void Init()
    {
      if (init)
        return;
      
      init = true;
      colorPaletteData = Addressables.LoadAssetAsync<ColorPaletteData>("ColorPaletteSettings");
      colorPaletteData.WaitForCompletion();
    }
    
    [MenuItem("Tools/Color Palette/Update Color Palette Data")]
    public static void UpdateColorPalette()
    {
      Init();
      
      List<ColorPaletteVo> vos = colorPaletteData.Result.colorPaletteVos;

      for (int i = 0; i < vos.Count; i++)
      {
        vos[i].name = i + 1 + ". Color Palette: " + vos[i].colorPaletteKey;

        for (int j = 0; j < vos[i].colorVos.Count; j++)
        {
          vos[i].colorVos[j].name = j + 1 + ". Color Key: " + vos[i].colorVos[j].colorKey;
        }
      }
    }
    
    [MenuItem("Tools/Color Palette/Lock Color Palette")]
    public static void Lock()
    {
      Init();
      
      colorPaletteData.Result.hideFlags = HideFlags.NotEditable;
    }
    
    [MenuItem("Tools/Color Palette/Unlock Color Palette")]
    public static void UnLock()
    {
      Init();

      colorPaletteData.Result.hideFlags = HideFlags.None;
    }
#endif
  }
}