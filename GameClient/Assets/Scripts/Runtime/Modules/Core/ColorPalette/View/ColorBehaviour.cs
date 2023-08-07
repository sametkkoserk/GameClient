using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.ColorPalette.Enum;
using Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Modules.Core.ColorPalette.View
{
  public class ColorBehaviour : MonoBehaviour
  {
    public ColorKey colorKey;
    
    private RawImage _rawImage;
    private Image _image;
    private bool _isRawImageNull;
    private bool _isImageNull;

    private void Start()
    {
      _image = gameObject.GetComponent<Image>();
      _rawImage = gameObject.GetComponent<RawImage>();
      _isImageNull = _image == null;
      _isRawImageNull = _rawImage == null;

      ColorPaletteModel.instance.BindMethod(ColorPaletteChanged);
      CheckAndChangeColor(ColorPaletteModel.instance.ChangeColor(colorKey));
    }

    private void ColorPaletteChanged()
    {
      CheckAndChangeColor(ColorPaletteModel.instance.ChangeColor(colorKey));
    }

    private void CheckAndChangeColor(Color color)
    {
      if (!_isRawImageNull)
      {
        _rawImage.color = color;
      }
      else if (!_isImageNull)
      {
        _image.color = color;
      }
      else
      {
        DebugX.Log(DebugKey.ColorPalette, "There are no Image and Raw Image Components! Object: " + gameObject.name, LogKey.Warning);
      }
    }
  }
}