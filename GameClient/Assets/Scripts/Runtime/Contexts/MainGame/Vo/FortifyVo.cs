using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class FortifyVo
  {
    [ProtoMember(1)]
    public int sourceCityId;
    
    [ProtoMember(2)]
    public int targetCityId;

    [ProtoMember(3)]
    public int soldierCount;
  }
}