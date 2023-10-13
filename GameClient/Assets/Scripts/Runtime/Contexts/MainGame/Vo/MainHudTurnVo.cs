using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.Network.Vo;

namespace Runtime.Contexts.MainGame.Vo
{
  public class MainHudTurnVo
  {
    public string title;
    
    public PlayerColorVo color;

    public ushort id;

    public NotificationPanelTypeKey panelTypeKey;

    public float time;
  }
}