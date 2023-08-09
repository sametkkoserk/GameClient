using Runtime.Contexts.Main.Command;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PlayerModel;
using Runtime.Contexts.Main.Model.PopupPanelModel;
using Runtime.Contexts.Main.Processor;
using Runtime.Contexts.Main.View.Application;
using Runtime.Contexts.Main.View.MainSceneCamera;
using Runtime.Contexts.Main.View.Notification.Model;
using Runtime.Contexts.Main.View.Notification.View;
using Runtime.Contexts.Main.View.Popup;
using Runtime.Contexts.Main.View.RegisterPanel;
using Runtime.Contexts.Main.View.Tooltip;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.Audio.Model.AudioModel.AudioModel;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using Runtime.Modules.Core.Discord.Model;
using Runtime.Modules.Core.Discord.View;
using Runtime.Modules.Core.GeneralContext;
using Runtime.Modules.Core.Icon.Model;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using Runtime.Modules.Core.ScreenManager.Enum;
using StrangeIoC.scripts.strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.Main.Config
{
    public class MainContext : GeneralContext
    {
        public MainContext (MonoBehaviour view) : base(view)
        {
        }

        public MainContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            base.mapBindings();
            
            injectionBinder.Bind<IBundleModel>().To<BundleModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICursorModel>().To<CursorModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IColorPaletteModel>().To<ColorPaletteModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IAudioModel>().To<AudioModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IDiscordModel>().To<DiscordModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalizationModel>().To<LocalizationModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPopupPanelModel>().To<PopupPanelModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<IIconModel>().To<IconModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<INotificationModel>().To<NotificationModel>().ToSingleton().CrossContext();

            mediationBinder.Bind<MainSceneCameraView>().To<MainSceneCameraMediator>();
            mediationBinder.Bind<RegisterPanelView>().To<RegisterPanelMediator>();
            mediationBinder.Bind<DiscordBehaviourView>().To<DiscordBehaviourMediator>();
            mediationBinder.Bind<ApplicationView>().To<ApplicationMediator>();
            mediationBinder.Bind<TooltipView>().To<TooltipMediator>();
            mediationBinder.Bind<PopupPanelView>().To<PopupPanelMediator>();
            mediationBinder.Bind<NotificationPanelView>().To<NotificationPanelMediator>();
            
            commandBinder.Bind(ContextEvent.START).InSequence()
                .To<LoadAssetsCommand>()
                .To<StartCommand>();
            
            commandBinder.Bind(PanelEvent.PanelContainersCreated).To<OpenRegisterPanelCommand>();
            
            commandBinder.Bind(MainEvent.StarterSettings).To<StarterSettingsCommand>();
            commandBinder.Bind(MainEvent.OpenSettingsPanel).To<OpenSettingsPanelCommand>();
            commandBinder.Bind(MainEvent.RegisterInfoSend).To<RegisterInfoSendCommand>();
            commandBinder.Bind(MainEvent.LanguageChanged).To<LanguageChangedCommand>();
            commandBinder.Bind(MainEvent.OpenPopupPanel).To<OpenPopupPanelCommand>();
            commandBinder.Bind(MainEvent.OpenNotificationPanel).To<OpenNotificationPanelCommand>();

            commandBinder.Bind(ServerToClientId.RegisterAccepted).To<RegisterAcceptedProcessor>();
        }
    }
}
