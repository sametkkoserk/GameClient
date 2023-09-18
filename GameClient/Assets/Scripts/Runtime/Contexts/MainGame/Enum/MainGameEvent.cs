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
    NextTurnNotificationPanel,

    RemainingTime,
    RemainingTimeMainHud,
    
    ChangeSizeOfPlayerList,
    
    ShowCityMiniInfoPanel,
    HideCityMiniInfoPanel,
    
    GameStateChanged,
    PlayerActionsChanged,
    PlayerActionsReferenceListExecuted,
    
    ClaimCity,
    ClaimedCity,
    
    UpdateDetailsPanel,
  }
}