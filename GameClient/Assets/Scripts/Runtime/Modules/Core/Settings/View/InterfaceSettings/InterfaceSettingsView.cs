using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;

namespace Runtime.Modules.Core.Settings.View.InterfaceSettings
{
  public class InterfaceSettingsView : EventView
  {
    public TMP_Dropdown colorPaletteDropdown;

    private void OnEnable()
    {
      dispatcher.Dispatch(InterfaceSettingsEvent.OnTabOpened);
    }

    public void OnColorPaletteDropdownChanged()
    {
      dispatcher.Dispatch(InterfaceSettingsEvent.ColorPaletteChanged, colorPaletteDropdown.value);
    }
  }
}