using Runtime.Contexts.MainGame.Command;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Processor;
using Runtime.Contexts.MainGame.View.City;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.View.MainMapContainer;
using Runtime.Contexts.MainGame.View.YourTurnPanel;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.GeneralContext;
using StrangeIoC.scripts.strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Config
{
  public class MainGameContext : GeneralContext
  {
    public MainGameContext(MonoBehaviour view) : base(view)
    {
    }

    public MainGameContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

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
      mediationBinder.Bind<MainGameManagerView>().To<MainGameManagerMediator>();
      mediationBinder.Bind<YourTurnPanelView>().To<YourTurnPanelMediator>();

      //Event/Command binding
      //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
      //The START event is fired as soon as mappings are complete.
      //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
      commandBinder.Bind(MainGameEvent.StartGame).To<CreateMapCommand>();

      commandBinder.Bind(ServerToClientId.GameStartPreparations).To<HandleMapGeneratorProcessor>();
      commandBinder.Bind(ServerToClientId.SendUserLobbyID).To<SendUserLobbyIDProcessor>();
    }
  }
}