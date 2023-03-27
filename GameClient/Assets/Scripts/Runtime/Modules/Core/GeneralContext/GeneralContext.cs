using Runtime.Contexts.Main.View.SettingsButton;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.View.LayerContainer;
using Runtime.Modules.Core.ScreenManager.View.PanelContainer;
using Runtime.Modules.Core.Settings.View.LanguageSettings;
using Runtime.Modules.Core.Settings.View.Settings;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Modules.Core.GeneralContext
{
  public class GeneralContext : MVCSContext
  {
    public GeneralContext(MonoBehaviour view) : base(view)
    {
    }

    public GeneralContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

      injectionBinder.Bind<IScreenManagerModel>().To<ScreenManagerModel>().ToSingleton();

      injectionBinder.Bind<ILocalizationModel>().To<LocalizationModel>().ToSingleton();


      mediationBinder.Bind<LayerContainerView>().To<LayerContainerMediator>();
      mediationBinder.Bind<PanelContainerView>().To<PanelContainerMediator>();

      mediationBinder.Bind<SettingsButtonView>().To<SettingsButtonMediator>();
      mediationBinder.Bind<SettingsPanelView>().To<SettingsPanelMediator>();

      mediationBinder.Bind<LanguageSettingsView>().To<LanguageSettingsMediator>();
    }
  }
}