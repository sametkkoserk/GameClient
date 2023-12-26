using Runtime.Contexts.MainGame.Command;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Processor;
using Runtime.Contexts.MainGame.View.ButtonPlayerActions;
using Runtime.Contexts.MainGame.View.City;
using Runtime.Contexts.MainGame.View.CityDetailsPanel;
using Runtime.Contexts.MainGame.View.CityMiniInfoPanel;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainGameNotificationPanel;
using Runtime.Contexts.MainGame.View.MainHudPanel;
using Runtime.Contexts.MainGame.View.MainHudPanel.Item;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.View.MainMapContainer;
using Runtime.Contexts.MainGame.View.MiniBottomPanel;
using Runtime.Contexts.MainGame.View.MiniGameStatsPanel;
using Runtime.Contexts.MainGame.View.MiniGameStatsPanel.Item;
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
      
      mediationBinder.Bind<CityView>().To<CityMediator>();
      mediationBinder.Bind<MainMapView>().To<MainMapMediator>();
      mediationBinder.Bind<MainMapContainerView>().To<MainMapContainerMediator>();
      mediationBinder.Bind<MainGameManagerView>().To<MainGameManagerMediator>();
      mediationBinder.Bind<MainHudPanelView>().To<MainHudPanelMediator>();
      mediationBinder.Bind<MainHudPanelPlayerItemView>().To<MainHudPanelPlayerItemMediator>();
      mediationBinder.Bind<CityMiniInfoPanelView>().To<CityMiniInfoPanelMediator>();
      mediationBinder.Bind<CityDetailsPanelView>().To<CityDetailsPanelMediator>();
      mediationBinder.Bind<PlayerActionsView>().To<PlayerActionsMediator>();
      mediationBinder.Bind<MainGameNotificationPanelView>().To<MainGameNotificationPanelMediator>();
      mediationBinder.Bind<MiniGameResultPanelView>().To<MiniGameResultPanelMediator>();
      mediationBinder.Bind<MiniGameResultPanelItemView>().To<MiniGameResultPanelItemMediator>();
      mediationBinder.Bind<MiniBottomPanelView>().To<MiniBottomPanelMediator>();

      commandBinder.Bind(ContextEvent.START).To<CreateMapCommand>();
      
      commandBinder.Bind(MainGameEvent.SceneReady).To<SceneReadyCommand>();
      commandBinder.Bind(MainGameEvent.ReadyToGameStart).To<ReadyToGameStartCommand>();
      commandBinder.Bind(MainGameEvent.ClaimCity).To<ClaimCityCommand>();
      commandBinder.Bind(MainGameEvent.ArmingToCity).To<ArmingToCityCommand>();
      commandBinder.Bind(MainGameEvent.ConfirmAttack).To<ConfirmAttackCommand>();
      commandBinder.Bind(MainGameEvent.ConfirmFortify).To<ConfirmFortifyCommand>();
      commandBinder.Bind(MainGameEvent.Pass).To<PassCommand>();

      commandBinder.Bind(ServerToClientId.GameStartPreparations).To<HandleMapGeneratorProcessor>();
      commandBinder.Bind(ServerToClientId.NextTurn).To<NextTurnProcessor>();
      commandBinder.Bind(ServerToClientId.RemainingTime).To<RemainingTimeProcessor>();
      commandBinder.Bind(ServerToClientId.GameStateChanged).To<GameStateChangedProcessor>();
      commandBinder.Bind(ServerToClientId.PlayerActionChanged).To<PlayerActionsChangedProcessor>();
      commandBinder.Bind(ServerToClientId.SendPlayerActionReference).To<SetAllPlayerActionReferenceProcessor>();
      commandBinder.Bind(ServerToClientId.UpdateCity).To<UpdateCityProcessor>();
      commandBinder.Bind(ServerToClientId.MiniGameRewards).To<MiniGameRewardsProcessor>();
      commandBinder.Bind(ServerToClientId.SendArmingCity).To<CompletedArmingProcessor>();

      commandBinder.Bind(ServerToClientId.Attack).To<AttackResultProcessor>();
      commandBinder.Bind(ServerToClientId.Fortify).To<FortifyResultProcessor>();

      commandBinder.Bind(ServerToClientId.CreateMiniGameScene).To<CreateMiniGameSceneProcessor>();

    }
  }
}