using Runtime.Lobby.Command;
using Runtime.Lobby.Enum;
using Runtime.Lobby.Model.LobbyModel;
using Runtime.Lobby.Processor;
using Runtime.Lobby.View.CreateLobbyPanel;
using Runtime.Lobby.View.JoinLobbyPanel;
using Runtime.Lobby.View.LobbyManagerPanel;
using Runtime.Lobby.View.LobbyPanel;
using Runtime.Lobby.View.LobbyUIManager;
using Runtime.Network.Enum;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Lobby.Config
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
            injectionBinder.Bind<ILobbyModel>().To<LobbyModel>().ToSingleton().CrossContext();
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
            mediationBinder.Bind<LobbyManagerPanelView>().To<LobbyManagerPanelMediator>();

            
            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            //commandBinder.Bind(ContextEvent.START).To<LoadNetworkSceneCommand>();
            commandBinder.Bind(LobbyEvent.SendCreateLobby).To<SendCreateLobbyCommand>();
            commandBinder.Bind(LobbyEvent.GetLobbies).To<GetLobbiesCommand>();
            commandBinder.Bind(LobbyEvent.JoinLobby).To<JoinLobbyCommand>();
            commandBinder.Bind(LobbyEvent.OutLobby).To<OutFromLobbyCommand>();
            commandBinder.Bind(LobbyEvent.PlayerReady).To<PlayerReadyCommand>();

            
            commandBinder.Bind(ServerToClientId.JoinedToLobby).To<JoinedToLobbyProcessor>();
            commandBinder.Bind(ServerToClientId.SendLobbies).To<GetLobbiesProcessor>();
            commandBinder.Bind(ServerToClientId.OutFromLobbyDone).To<OutFromLobbyDoneProcessor>();
            commandBinder.Bind(ServerToClientId.NewPlayerToLobby).To<NewPlayerToLobbyProccessor>();
            commandBinder.Bind(ServerToClientId.PlayerReadyResponse).To<PlayerReadyResponseProcessor>();
        }
    }
}

