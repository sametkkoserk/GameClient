using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class ArmingVo
  {
    [ProtoMember(1)]
    public CityVo cityVo;

    [ProtoMember(2)]
    public int soldierCount;
  }
}