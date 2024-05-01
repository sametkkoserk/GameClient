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
    
    [ProtoMember(3)]
    public string email;
    
    [ProtoMember(4)]
    public string password;
  }
}