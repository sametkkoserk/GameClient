using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameStartVo
  {
    [ProtoMember(1)]
    public bool gameStart;
    [ProtoMember(2)]
    public ushort lobbyId;
  }
}