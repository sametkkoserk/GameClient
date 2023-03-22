using Runtime.Contexts.Network.Command;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.View.NetworkManager;
using Runtime.Modules.Core.GeneralContext;
using StrangeIoC.scripts.strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.Network.Config
{
  public class NetworkContext : GeneralContext
  {
    public NetworkContext(MonoBehaviour view) : base(view)
    {
    }

    public NetworkContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

      //Injection binding.
      //Map a mock model and a mock service, both as Singletons
      //injectionBinder.Bind<IExampleModel>().To<ExampleModel>().ToSingleton();
      injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton().CrossContext();

      //View/Mediator binding
      //This Binding instantiates a new ExampleMediator whenever as ExampleView
      //Fires its Awake method. The Mediator communicates to/from the View
      //and to/from the App. This keeps dependencies between the view and the app
      //separated.
      mediationBinder.Bind<NetworkManagerView>().To<NetworkManagerMediator>();

      //Event/Command binding
      //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
      //The START event is fired as soon as mappings are complete.
      //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
      commandBinder.Bind(ContextEvent.START).InSequence()
        .To<ClientConnectCommand>()
        .To<CreateLobbyContextCommand>();
    }
  }
}