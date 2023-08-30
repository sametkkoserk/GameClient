using System;
using System.Collections.Generic;
using ProtoBuf;
using Runtime.Contexts.MainGame.Enum;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  [Serializable]
  public class PlayerActionPermissionReferenceVo
  {
    [ProtoMember(1)]
    public List<GameStateKey> gameStateKeys = new();

    [ProtoMember(2)]
    public List<PlayerActionKey> playerActionNecessaryKeys = new();
  }
}