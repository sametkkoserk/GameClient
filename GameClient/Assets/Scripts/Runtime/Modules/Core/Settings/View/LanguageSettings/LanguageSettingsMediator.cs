using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Modules.Core.Settings.View.LanguageSettings
{
  public enum LanguageSettingsEvent
  {
    ChangeLanguage
  }

  public class LanguageSettingsMediator : EventMediator
  {
    [Inject]
    public LanguageSettingsView view { get; set; }

    [Inject]
    public ILocalizationModel localizationModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LanguageSettingsEvent.ChangeLanguage, OnChangeLanguage);

      Init();
    }

    private void Init()
    {
      switch (localizationModel.GetLanguageCode())
      {
        case LanguageKey.en:
          view.dropdown.SetValueWithoutNotify(0);
          break;
        case LanguageKey.tr:
          view.dropdown.SetValueWithoutNotify(1);
          break;
      }
    }

    private void OnChangeLanguage(IEvent payload)
    {
      string language = (string)payload.data;

      switch (language)
      {
        case "English":
          StartCoroutine(localizationModel.ChangeLanguage(LanguageKey.en));
          break;
        case "Türkçe":
          StartCoroutine(localizationModel.ChangeLanguage(LanguageKey.tr));
          break;
      }
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LanguageSettingsEvent.ChangeLanguage, OnChangeLanguage);
    }
  }
}