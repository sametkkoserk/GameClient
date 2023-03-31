using Runtime.Contexts.Main.Command;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.View.MainSceneCamera;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using Runtime.Modules.Core.GeneralContext;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
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
            //Injection binding.
            //Map a mock model and a mock service, both as Singletons
            //injectionBinder.Bind<IExampleModel>().To<ExampleModel>().ToSingleton();
            //injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();

            //View/Mediator binding
            //This Binding instantiates a new ExampleMediator whenever as ExampleView
            //Fires its Awake method. The Mediator communicates to/from the View
            //and to/from the App. This keeps dependencies between the view and the app
            //separated.
            injectionBinder.Bind<IBundleModel>().To<BundleModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICursorModel>().To<CursorModel>().ToSingleton().CrossContext();
            injectionBinder.Bind<ILocalizationModel>().To<LocalizationModel>().ToSingleton().CrossContext();

            mediationBinder.Bind<MainSceneCameraView>().To<MainSceneCameraMediator>();
            
            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            commandBinder.Bind(ContextEvent.START).InSequence()
                .To<LoadAssetsCommand>()
                .To<StartCommand>();
            
            commandBinder.Bind(MainEvent.StarterSettings).To<StarterSettingsCommand>();
            commandBinder.Bind(MainEvent.OpenSettingsPanel).To<OpenSettingsPanelCommand>();
        }
    }
}
