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
    ShowSelectorPartInBottomPanel,
    DisappearBottomPanel,
    
    GameStateChanged,
    
    ClaimCity,
    ClaimedCity,
    ArmingToCity,
    SelectCityToAttack,
    ConfirmAttack,
    AttackResult,
    SetTransferSoldierAfterAttack,
    SetTransferSoldierForFortify,
    Fortify,
    ConfirmFortify,
    FortifyResult,
    ResetCityMode,
    OutlineSettings,
    
    ToTheMiniGame,
    OpenMiniGameResultPanel,
  }
}