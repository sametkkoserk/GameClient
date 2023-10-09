using Assets.SimpleLocalization.Scripts;
using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.Audio.Enum;
using Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel;
using Runtime.Modules.Core.ColorPalette.Enum;
using Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using Runtime.Modules.Core.Discord.Model;

using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.Settings.Enum;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Main.Command
{
  public class StarterSettingsCommand : EventCommand
  {

    [Inject]
    public ICursorModel cursorModel { get; set; }
    
    [Inject]
    public IColorPaletteModel colorPaletteModel { get; set; }
    
    [Inject]
    public IAudioModel audioModel { get; set; }
    
    [Inject]
    public IDiscordModel discordModel { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }
    
    public override void Execute()
    {
      CursorSettings();
      ColorPaletteSettings();
      CursorSettings();
      StartMusic();
      InitDiscord();
      OpenTooltipPanel();
    }

    private void CursorSettings()
    {
      cursorModel.OnChangeCursor(CursorKey.Default);
    }

    private void ColorPaletteSettings()
    {
      int value = PlayerPrefs.GetInt(SettingsSaveKey.ColorPalette.ToString(), 0);
      colorPaletteModel.ChangeColorPalette((ColorPaletteKey)value);
    }

    private void StartMusic()
    {
      audioModel.ChangeMasterVolume(PlayerPrefs.GetFloat(SettingsSaveKey.MasterVolume.ToString(), 1f));
      audioModel.ChangeMusicVolume(PlayerPrefs.GetFloat(SettingsSaveKey.MusicVolume.ToString(), 1f));
      audioModel.ChangeUISoundVolume(PlayerPrefs.GetFloat(SettingsSaveKey.UIVolume.ToString(), 1f));
      
      audioModel.PlayMusic(MusicSoundsKey.StreetLove);
    }

    private void InitDiscord()
    {
      discordModel.Init();
      
      discordModel.StarterSettings();
    }

    private void OpenTooltipPanel()
    {
      screenManagerModel.OpenPanel(MainPanelKey.TooltipPanel, SceneKey.Main, LayerKey.TooltipLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }
  }
}