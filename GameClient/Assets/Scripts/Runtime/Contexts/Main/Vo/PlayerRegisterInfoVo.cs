using ProtoBuf;

namespace Runtime.Contexts.Main.Vo
{
  [ProtoContract]
  public class PlayerRegisterInfoVo
  {
    [ProtoMember(1)]
    public ushort userId;
    
    [ProtoMember(2)]
    public string username;
  }
}