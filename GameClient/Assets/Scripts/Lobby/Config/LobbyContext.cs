using Lobby.Command;
using Lobby.Enum;
using Lobby.Model.LobbyModel;
using Lobby.Processor;
using Lobby.View.CreateLobbyPanel;
using Lobby.View.JoinLobbyPanel;
using Lobby.View.LobbyPanel;
using Lobby.View.LobbyUIManager;
using Main.Command;
using Network.Enum;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Lobby.Config
{
    public class LobbyContext : MVCSContext
    {
        public LobbyContext (MonoBehaviour view) : base(view)
        {
        }

        public LobbyContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            //Injection binding.
            //Map a mock model and a mock service, both as Singletons
            injectionBinder.Bind<ILobbyModel>().To<LobbyModel>().ToSingleton();
            //injectionBinder.Bind<INetworkManagerService>().To<NetworkManagerService>().ToSingleton();
            //View/Mediator binding
            //This Binding instantiates a new ExampleMediator whenever as ExampleView
            //Fires its Awake method. The Mediator communicates to/from the View
            //and to/from the App. This keeps dependencies between the view and the app
            //separated.
            mediationBinder.Bind<CreateLobbyPanelView>().To<CreateLobbyPanelMediator>();
            mediationBinder.Bind<JoinLobbyPanelView>().To<JoinLobbyPanelMediator>();
            mediationBinder.Bind<LobbyUIManagerView>().To<LobbyUIManagerMediator>();
            mediationBinder.Bind<LobbyPanelView>().To<LobbyPanelMediator>();

            
            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            //commandBinder.Bind(ContextEvent.START).To<LoadNetworkSceneCommand>();
            commandBinder.Bind(LobbyEvent.SendCreateLobby).To<SendCreateLobbyCommand>();
            commandBinder.Bind(LobbyEvent.GetLobbies).To<GetLobbiesCommand>();
            commandBinder.Bind(LobbyEvent.JoinLobby).To<JoinLobbyCommand>();

            
            commandBinder.Bind(ServerToClientId.JoinedToLobby).To<JoinedToLobbyProcessor>();
            commandBinder.Bind(ServerToClientId.SendLobbies).To<GetLobbiesProcessor>();
        }
    }
}

