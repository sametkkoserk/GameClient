using System.Collections;
using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.Settings.Enum;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

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

    public override void OnRegister()
    {
      Init();
    }

    private void Init()
    {
    }



    private IEnumerator WaitChangingLanguage()
    {
      yield return new WaitForSecondsRealtime(5f);
      dispatcher.Dispatch(MainEvent.LanguageChanged);
    }

    public override void OnRemove()
    {

    }
  }
}