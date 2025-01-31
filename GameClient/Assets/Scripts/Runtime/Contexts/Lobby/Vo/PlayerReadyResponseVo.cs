using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class PlayerReadyResponseVo
  {
    [ProtoMember(1)]
    public ushort inLobbyId;
    [ProtoMember(2)]
    public ushort lobbyId;
    [ProtoMember(3)]
    public bool startGame;
  }
}