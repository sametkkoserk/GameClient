using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class NextTurnVo
  {
    [ProtoMember(1)]
    public ushort currentTurnPlayerLobbyId;
    [ProtoMember(2)]
    public ushort lobbyId;
  }
}