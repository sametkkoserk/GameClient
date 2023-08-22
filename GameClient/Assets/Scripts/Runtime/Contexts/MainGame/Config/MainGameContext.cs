using Runtime.Contexts.MainGame.Command;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Processor;
using Runtime.Contexts.MainGame.View.City;
using Runtime.Contexts.MainGame.View.CityMiniInfoPanel;
using Runtime.Contexts.MainGame.View.MainGameManager;
using Runtime.Contexts.MainGame.View.MainHudPanel;
using Runtime.Contexts.MainGame.View.MainHudPanel.Item;
using Runtime.Contexts.MainGame.View.MainMap;
using Runtime.Contexts.MainGame.View.MainMapContainer;
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

      commandBinder.Bind(MainGameEvent.StartGame).To<CreateMapCommand>();
      commandBinder.Bind(MainGameEvent.ReadyToGameStart).To<ReadyToGameStartCommand>();

      commandBinder.Bind(ServerToClientId.GameStartPreparations).To<HandleMapGeneratorProcessor>();
      commandBinder.Bind(ServerToClientId.NextTurn).To<NextTurnProcessor>();
      commandBinder.Bind(ServerToClientId.RemainingTime).To<RemainingTimeProcessor>();
    }
  }
}