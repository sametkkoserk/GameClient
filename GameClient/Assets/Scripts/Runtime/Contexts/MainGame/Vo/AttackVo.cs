using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class AttackVo
  {
    [ProtoMember(1)]
    public CityVo attackerCityVo;
    
    [ProtoMember(2)]
    public CityVo defenderCityVo;
  }
}