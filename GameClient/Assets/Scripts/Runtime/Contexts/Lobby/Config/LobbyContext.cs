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
using StrangeIoC.scripts.strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Config
{
  public class LobbyContext : GeneralContext
  {
    public LobbyContext(MonoBehaviour view) : base(view)
    {
    }

    public LobbyContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();
      
      injectionBinder.Bind<ILobbyModel>().To<LobbyModel>().ToSingleton().CrossContext();

      mediationBinder.Bind<CreateLobbyPanelView>().To<CreateLobbyPanelMediator>();
      mediationBinder.Bind<JoinLobbyPanelView>().To<JoinLobbyPanelMediator>();
      mediationBinder.Bind<LobbyPanelView>().To<LobbyPanelMediator>();
      mediationBinder.Bind<LobbyManagerPanelView>().To<LobbyManagerPanelMediator>();

      commandBinder.Bind(LobbyEvent.LoginOrRegisterCompletedSuccessfully).To<StartLobbyCommand>();
      commandBinder.Bind(LobbyEvent.SendCreateLobby).To<SendCreateLobbyCommand>();
      commandBinder.Bind(LobbyEvent.GetLobbies).To<GetLobbiesCommand>();
      commandBinder.Bind(LobbyEvent.JoinLobby).To<JoinLobbyCommand>();
      commandBinder.Bind(LobbyEvent.QuitLobby).To<QuitFromLobbyCommand>();
      commandBinder.Bind(LobbyEvent.PlayerReady).To<PlayerReadyCommand>();
      commandBinder.Bind(LobbyEvent.AddBot).To<AddBotCommand>();
      commandBinder.Bind(LobbyEvent.GameSettingsChanged).To<GameSettingsChangedCommand>();

      commandBinder.Bind(ServerToClientId.JoinedToLobby).To<JoinedToLobbyProcessor>();
      commandBinder.Bind(ServerToClientId.SendLobbies).To<GetLobbiesProcessor>();
      commandBinder.Bind(ServerToClientId.QuitFromLobbyDone).To<QuitFromLobbyDoneProcessor>();
      commandBinder.Bind(ServerToClientId.NewPlayerToLobby).To<NewPlayerToLobbyProcessor>();
      commandBinder.Bind(ServerToClientId.PlayerReadyResponse).To<PlayerReadyResponseProcessor>();
      commandBinder.Bind(ServerToClientId.GameSettingsChanged).To<GetGameSettingsProcessor>();
      commandBinder.Bind(ServerToClientId.LobbyIsClosed).To<LobbyIsClosedProcessor>();
    }
  }
}