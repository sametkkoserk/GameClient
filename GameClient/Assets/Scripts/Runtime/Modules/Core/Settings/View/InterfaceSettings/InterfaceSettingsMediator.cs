using System.Collections.Generic;
using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using Runtime.Modules.Core.ColorPalette.Enum;
using Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel;
using Runtime.Modules.Core.Settings.Enum;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;

namespace Runtime.Modules.Core.Settings.View.InterfaceSettings
{
  public enum InterfaceSettingsEvent
  {
    ColorPaletteChanged,
    OnTabOpened
  }
  public class InterfaceSettingsMediator : EventMediator
  {
    [Inject]
    public InterfaceSettingsView view { get; set; }
    
    [Inject]
    public IColorPaletteModel colorPaletteModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(InterfaceSettingsEvent.ColorPaletteChanged, OnColorPaletteChanged);
      view.dispatcher.AddListener(InterfaceSettingsEvent.OnTabOpened, OnTabOpened);
      
      Init();
    }
    
    private void Init()
    {
      SetColorPaletteDropdownValues();
      SetStartValueColorPaletteDropdown();
    }

    private void OnTabOpened()
    {
      SetColorPaletteDropdownValues();
      SetStartValueColorPaletteDropdown();
    }
    
    public void SetColorPaletteDropdownValues()
    {
      view.colorPaletteDropdown.ClearOptions();

      string[] paletteKeys = System.Enum.GetNames(typeof(ColorPaletteKey));

      for (int i = 0; i < paletteKeys.Length; i++)
      {
        TMP_Dropdown.OptionData option = new()
        {
          text = LocalizationManager.Localize("SettingsPanelCPD" + paletteKeys[i])
        };
        
        view.colorPaletteDropdown.options.Add(option);
      }
    }

    private void SetStartValueColorPaletteDropdown()
    {
      List<TMP_Dropdown.OptionData> optionsList = view.colorPaletteDropdown.options;

      ColorPaletteKey currentColorPaletteKey = colorPaletteModel.GetColorPalette();

      int index = (int) currentColorPaletteKey;

      for (int i = 0; i < optionsList.Count; i++)
      {
        if (i != index) continue;
        view.colorPaletteDropdown.SetValueWithoutNotify(i);
      }
    }
    private void OnColorPaletteChanged(IEvent payload)
    {
      int newColorPaletteKey = (int)payload.data;

      ColorPaletteKey newKey = (ColorPaletteKey) newColorPaletteKey;
      
      colorPaletteModel.ChangeColorPalette(newKey);
    }

    private void SetPlayerPrefs()
    {
      PlayerPrefs.SetInt(SettingsSaveKey.ColorPalette.ToString(), (int)colorPaletteModel.GetColorPalette());
    }
    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(InterfaceSettingsEvent.ColorPaletteChanged, OnColorPaletteChanged);
      view.dispatcher.RemoveListener(InterfaceSettingsEvent.OnTabOpened, OnTabOpened);
      
      SetPlayerPrefs();
    }
  }
}