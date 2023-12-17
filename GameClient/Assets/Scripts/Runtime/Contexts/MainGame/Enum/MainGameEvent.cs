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
    UpdateDetailsPanel,
    
    GameStateChanged,
    PlayerActionsChanged,
    PlayerActionsReferenceListExecuted,
    
    ClaimCity,
    ClaimedCity,
    
    ArmingToCity,
    SelectCityToAttack,
    ConfirmAttack,
    AttackResult,
    Fortify,
    ConfirmFortify,
    FortifyResult,
    ResetCityMode,
    
    OpenMiniGameResultPanel,
  }
}