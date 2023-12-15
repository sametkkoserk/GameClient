using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class FortifyVo
  {
    [ProtoMember(1)]
    public CityVo sourceCityVo;
    
    [ProtoMember(2)]
    public CityVo targetCityVo;
  }
}