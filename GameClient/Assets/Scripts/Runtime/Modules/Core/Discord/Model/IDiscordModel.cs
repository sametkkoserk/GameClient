using Runtime.Modules.Core.Discord.Vo;

namespace Runtime.Modules.Core.Discord.Model
{
  public interface IDiscordModel
  {
    public int lastState { get; }
    public void Init();

    public void SetStatus(DiscordInfoVo vo);

    public void OnClearStatus();

    public void NetworkUpdateMethod();

    public void NetworkLateUpdateMethod();

    public void ClearModel();

    public void Dispose();
    
    public void StarterSettings();

    public void OnMenu(string username);

    public void InLobby(string username, int playerCount, int maxPlayerCount);

    public void InGame(string username);
  }
}