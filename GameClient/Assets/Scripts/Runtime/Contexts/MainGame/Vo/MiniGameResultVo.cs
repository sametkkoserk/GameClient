using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class MiniGameResultVo
  {
    [ProtoMember(1)]
    public ushort id;
    
    [ProtoMember(2)]
    public ushort playerArrangement;

    [ProtoMember(3)]
    public List<string> playerRewards;
    
    [ProtoMember(4)]
    public PlayerFeaturesVo playerFeaturesVo;
  }
}