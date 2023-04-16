using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class OutFromLobbyVo
  {
    [ProtoMember(1)]
    public ushort clientId;
    [ProtoMember(2)]
    public ushort inLobbyId;
    [ProtoMember(3)]
    public ushort lobbyId;
  }
}