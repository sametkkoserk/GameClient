using Runtime.Modules.Core.ColorPalette.Enum;
using Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class StarterSettingsCommand : EventCommand
  {
    [Inject]
    public ILocalizationModel localizationModel { get; set; }
    
    [Inject]
    public ICursorModel cursorModel { get; set; }
    
    [Inject]
    public IColorPaletteModel colorPaletteModel { get; set; }
    public override void Execute()
    {
      LanguageSettings();
      ColorPaletteSettings();
      CursorSettings();
    }

    private void CursorSettings()
    {
      cursorModel.OnChangeCursor(CursorKey.Default);
    }

    private void LanguageSettings()
    {
      // In the future it will be from database or player pref.
      localizationModel.ChangeLanguage(LanguageKey.en);
    }

    private void ColorPaletteSettings()
    {
      colorPaletteModel.ChangeColorPalette(ColorPaletteKey.Standard);
    }
  }
}