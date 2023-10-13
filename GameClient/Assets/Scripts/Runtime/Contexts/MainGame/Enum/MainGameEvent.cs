namespace Runtime.Contexts.MainGame.Enum
{
  public enum MainGameEvent
  {
    SceneReady,
    StartGame,
    CreateMap,
    MapGenerator,
    ReadyToGameStart,
    
    NextTurn,
    NextTurnMainHud,
    NotificationPanel,

    RemainingTime,
    RemainingTimeMainHud,
    StopTimer,
    
    ChangeSizeOfPlayerList,
    
    ShowCityMiniInfoPanel,
    HideCityMiniInfoPanel,
    CityDetailsPanelClosed,
    
    GameStateChanged,
    PlayerActionsChanged,
    PlayerActionsReferenceListExecuted,
    
    ClaimCity,
    ClaimedCity,
    
    ArmingToCity,
    
    UpdateDetailsPanel,
    
    OpenMiniGameResultPanel,
    
  }
}