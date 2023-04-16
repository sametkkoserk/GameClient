using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.Lobby.Vo
{
  [ProtoContract]
  public class LobbiesVo
  {
    [ProtoMember(1)]
    public Dictionary<ushort, LobbyVo> lobbies { get; set; }
  }
}