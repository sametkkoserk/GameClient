using System.Collections.Generic;
using ProtoBuf;

namespace Runtime.Contexts.MainGame.Vo
{
  [ProtoContract]
  public class MapGeneratorVo
  {
    [ProtoMember(1)]
    public Dictionary<int, CityVo> cityVos;
  }
}