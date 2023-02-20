using MainGame.Command;
using MainGame.Enum;
using MainGame.Model;
using MainGame.Processor;
using MainGame.View.City;
using MainGame.View.MainMap;
using MainGame.View.MainMapContainer;
using Network.Enum;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace MainGame.Config
{
    public class MainGameContext : MVCSContext
    {
        public MainGameContext (MonoBehaviour view) : base(view)
        {
        }

        public MainGameContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            injectionBinder.Bind<IMainGameModel>().To<MainGameModel>().ToSingleton();
            //Injection binding.
            //Map a mock model and a mock service, both as Singletons
            //injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();
            //View/Mediator binding
            //This Binding instantiates a new ExampleMediator whenever as ExampleView
            //Fires its Awake method. The Mediator communicates to/from the View
            //and to/from the App. This keeps dependencies between the view and the app
            //separated.
            mediationBinder.Bind<CityView>().To<CityMediator>();
            mediationBinder.Bind<MainMapView>().To<MainMapMediator>();
            mediationBinder.Bind<MainMapContainerView>().To<MainMapContainerMediator>();


            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            commandBinder.Bind(MainGameEvent.StartGame).To<CreateMapCommand>();
            commandBinder.Bind(ServerToClientId.SendMap).To<HandleMapGeneratorProcessor>();
        }
    }
}

