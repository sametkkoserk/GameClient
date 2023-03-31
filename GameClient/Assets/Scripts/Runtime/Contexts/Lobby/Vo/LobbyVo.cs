using System.Collections.Generic;

namespace Runtime.Contexts.Lobby.Vo
{
  public class LobbyVo
  {
    public Dictionary<ushort, ClientVo> clients;

    public bool isPrivate;

    public ushort leaderId;
    public ushort lobbyId;

    public string lobbyName;

    public LobbySettingsVo lobbySettingsVo;

    public ushort maxPlayerCount;

    public ushort playerCount;

    public ushort readyCount;
  }
}