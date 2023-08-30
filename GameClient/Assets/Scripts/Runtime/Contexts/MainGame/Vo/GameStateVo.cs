using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class GameStateVo
  {
    [ProtoMember(1)]
    public GameStateKey gameStateKey;

    [ProtoMember(2)]
    public Dictionary<ushort, ClientVo> clients;
  }
}