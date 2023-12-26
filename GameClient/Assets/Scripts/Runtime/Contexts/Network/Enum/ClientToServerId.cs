namespace Runtime.Contexts.Network.Enum
{
  /// <summary>
  /// Integer value automatically attached, there is no necessary manually set. Also, it must be same with file in the server.
  /// </summary>
  public enum ClientToServerId : ushort
  {
    Register,
    
    CreateLobby,
    GetLobbies,
    JoinLobby,
    QuitFromLobby,
    
    PlayerReady,
    GameSettingsChanged,
    GameStart,
    SceneReady,
    
    Pass,
    ClaimCity,
    ArmingToCity,
    Attack,
    Fortify,
    ButtonClicked
  }
}