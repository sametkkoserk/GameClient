using Network.Command;
using Network.Enum;
using Network.Processor;
using Network.Services.NetworkManager;
using Network.View.NetworkManager;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Network.Config
{
    public class NetworkContext : MVCSContext
    {
        public NetworkContext (MonoBehaviour view) : base(view)
        {
        }

        public NetworkContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
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
            commandBinder.Bind(ContextEvent.START).To<ClientConnectCommand>();
            commandBinder.Bind(NetworkEvent.SendMessage).To<SendMessageCommand>();
            commandBinder.Bind(NetworkEvent.CreateLobby).To<CreateLobbyContextCommand>();

            
            commandBinder.Bind(ServerToClientId.response).To<HandleResponseProcessor>();


        }
    }
}

