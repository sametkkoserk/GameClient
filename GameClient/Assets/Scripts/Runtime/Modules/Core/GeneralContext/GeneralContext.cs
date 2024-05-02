using Runtime.Contexts.Main.View.SettingsButton;
using Runtime.Contexts.Main.View.TooltipManager;
using Runtime.Modules.Core.Audio.View.AudioSourceItem;
using Runtime.Modules.Core.Audio.View.SourceCreator;
using Runtime.Modules.Core.Icon.View;
using Runtime.Modules.Core.RootManager;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using Runtime.Modules.Core.ScreenManager.View.LayerContainer;
using Runtime.Modules.Core.ScreenManager.View.PanelContainer;
using Runtime.Modules.Core.Settings.View.AudioSettings;
using Runtime.Modules.Core.Settings.View.InterfaceSettings;
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

      mediationBinder.Bind<LayerContainerView>().To<LayerContainerMediator>();
      mediationBinder.Bind<PanelContainerView>().To<PanelContainerMediator>();
      mediationBinder.Bind<RootManagerView>().To<RootManagerMediator>();
      
      mediationBinder.Bind<SettingsButtonView>().To<SettingsButtonMediator>();
      mediationBinder.Bind<SettingsPanelView>().To<SettingsPanelMediator>();
      
      mediationBinder.Bind<InterfaceSettingsView>().To<InterfaceSettingsMediator>();
      mediationBinder.Bind<AudioSettingsView>().To<AudioSettingsMediator>();
      
      mediationBinder.Bind<AudioSourceCreatorView>().To<AudioSourceCreatorMediator>();
      mediationBinder.Bind<AudioSourceItemView>().To<AudioSourceItemMediator>();
      
      mediationBinder.Bind<TooltipTriggerView>().To<TooltipTriggerMediator>();

      mediationBinder.Bind<IconView>().To<IconMediator>();
    }
  }
}