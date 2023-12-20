namespace Runtime.Contexts.MainGame.Enum
{
  public enum MainGameEvent
  {
    SceneReady,
    StartGame,
    CreateMap,
    MapGenerator,
    ReadyToGameStart,
    
    Pass,
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
    ShowHideMiniBottomPanel,
    
    GameStateChanged,
    PlayerActionsChanged,
    PlayerActionsReferenceListExecuted,
    
    ClaimCity,
    ClaimedCity,
    ArmingToCity,
    SelectCityToAttack,
    ConfirmAttack,
    AttackResult,
    SetTransferSoldierAfterAttack,
    Fortify,
    ConfirmFortify,
    FortifyResult,
    ResetCityMode,
    
    OpenMiniGameResultPanel,
  }
}