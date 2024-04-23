using Runtime.Contexts.MainGame.Processor;
using Runtime.Contexts.MiniGames.Command;
using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Processor;
using Runtime.Contexts.MiniGames.View.GameSelectionPanel;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.View.MiniGameContainer;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Modules.Core.GeneralContext;
using StrangeIoC.scripts.strange.extensions.context.api;
using UnityEngine;

namespace Runtime.Contexts.MiniGames.Config
{
  public class MiniGamesContext : GeneralContext
  {
    public MiniGamesContext(MonoBehaviour view) : base(view)
    {
    }

    public MiniGamesContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

      injectionBinder.Bind<IMiniGamesModel>().To<MiniGamesModel>().ToSingleton();

      mediationBinder.Bind<MiniGameContainerView>().To<MiniGameContainerMediator>();
      mediationBinder.Bind<MiniGameView>().To<MiniGameMediator>();
      mediationBinder.Bind<GameSelectionPanelView>().To<GameSelectionPanelMediator>();
      
      commandBinder.Bind(MiniGamesEvent.ButtonClicked).To<ButtonClickedCommand>();
      commandBinder.Bind(MiniGamesEvent.SceneReady).To<SceneReadyCommand>();
      commandBinder.Bind(MiniGamesEvent.MiniGameCreated).To<MiniGameCreatedCommand>();
      
      commandBinder.Bind(ServerToClientId.MiniGameCreated).To<OnMiniGameCreateProcessor>();
      commandBinder.Bind(ServerToClientId.SendMiniGameState).To<OnMiniGameStateProcessor>();
      commandBinder.Bind(ServerToClientId.MiniGameEnded).To<OnMiniGameEndedProcessor>();
      commandBinder.Bind(ServerToClientId.SendMiniGameMap).To<OnMiniGameMapProcessor>();


    }
  }
}