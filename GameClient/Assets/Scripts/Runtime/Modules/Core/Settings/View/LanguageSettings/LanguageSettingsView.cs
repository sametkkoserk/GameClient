using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;

namespace Runtime.Modules.Core.Settings.View.LanguageSettings
{
  public class LanguageSettingsView : EventView
  {
    public TMP_Dropdown dropdown;

    public void OnDropdownValueChanged()
    {
      dispatcher.Dispatch(LanguageSettingsEvent.ChangeLanguage, dropdown.options[dropdown.value].text);
    }
  }
}