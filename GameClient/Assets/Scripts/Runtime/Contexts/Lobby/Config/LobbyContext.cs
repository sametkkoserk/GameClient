using Runtime.Contexts.Lobby.Command;
using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Processor;
using Runtime.Contexts.Lobby.View.CreateLobbyPanel;
using Runtime.Contexts.Lobby.View.JoinLobbyPanel;
using Runtime.Contexts.Lobby.View.LobbyManagerPanel;
using Runtime.Contexts.Lobby.View.LobbyPanel;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.GeneralContext;
using Runtime.Modules.Core.ScreenManager.Enum;
using strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Config
{
    public class LobbyContext : GeneralContext
    {
        public LobbyContext (MonoBehaviour view) : base(view)
        {
        }

        public LobbyContext (MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }
        
        protected override void mapBindings()
        {
            base.mapBindings();
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
            mediationBinder.Bind<LobbyPanelView>().To<LobbyPanelMediator>();
            mediationBinder.Bind<LobbyManagerPanelView>().To<LobbyManagerPanelMediator>();
            
            //Event/Command binding
            //commandBinder.Bind(ExampleEvent.REQUEST_WEB_SERVICE).To<CallWebServiceCommand>();
            //The START event is fired as soon as mappings are complete.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            // commandBinder.Bind(ContextEvent.START).To<StartLobbyCommand>();

            commandBinder.Bind(PanelEvent.PanelContainersCreated).To<StartLobbyCommand>();
            
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

