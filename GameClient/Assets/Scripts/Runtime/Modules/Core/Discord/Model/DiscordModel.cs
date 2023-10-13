using System;
using Assets.SimpleLocalization.Scripts;
using Discord;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.Discord.Vo;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Modules.Core.Discord.Model
{
  public class DiscordModel : IDiscordModel
  {
    public const long applicationID = 1157215237460738118;

    public const string gameLogo = "kiesmagame";

    public ActivityManager activityManager;

    public global::Discord.Discord discord;

    public LobbyManager lobbyManager;

    private long _time;

    public static IDiscordModel instance;

    public int lastState { get; set; }

    private string _username = "";

    [PostConstruct]
    public void OnPostConstruct()
    {
      instance = this;
    }
    public void Init()
    {
      try
      {
        discord = new global::Discord.Discord(applicationID, (ulong)CreateFlags.NoRequireDiscord);
      }
      catch (Exception e)
      {
        DebugX.Log(DebugKey.Discord, "Discord is closed. Features disabled. " + e);
        ClearModel();
        return;
      }

      try
      {
        lobbyManager = discord.GetLobbyManager();
      }
      catch (Exception e)
      {
        DebugX.Log(DebugKey.Discord, e.Message);

        ClearModel();
        return;
      }

      activityManager = discord.GetActivityManager();
    }

    public void SetStatus(DiscordInfoVo vo)
    {
      if (activityManager == null && discord == null)
      {
        return;
      }

      Activity activity = new()
      {
        Details = vo.details,
        State = vo.state,
        Assets =
        {
          LargeImage = gameLogo,
          LargeText = vo.largeText
        },
        Type = ActivityType.Playing,
        Instance = true
      };

      if (vo.timer)
      {
        _time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        activity.Timestamps.Start = _time;
      }

      activityManager.UpdateActivity(activity, result =>
      {
        if (result != Result.Ok)
        {
          DebugX.Log(DebugKey.Discord, "Failed connecting to Discord!", LogKey.Warning);
        }
      });
    }

    public void OnClearStatus()
    {
      activityManager.ClearActivity(result =>
      {
        if (result == Result.Ok)
        {
          DebugX.Log(DebugKey.Discord, "Discord activity cleared.");
        }
        else
        {
          DebugX.Log(DebugKey.Discord, "An error occurred while clearing the Discord activity: " + result, LogKey.Warning);
        }
      });
    }

    public void NetworkUpdateMethod()
    {
      try
      {
        discord?.RunCallbacks();
      }
      catch (Exception e)
      {
        DebugX.Log(DebugKey.Discord, e.Message);
        ClearModel();
      }
    }

    public void NetworkLateUpdateMethod()
    {
      lobbyManager?.FlushNetwork();
    }

    public void Dispose()
    {
      OnClearStatus();
      discord?.Dispose();
    }

    public void ClearModel()
    {
      activityManager = null;
      discord = null;
      lobbyManager = null;
    }

    public void SetStatusInfo(string _details, string _state, bool _timer)
    {
      DiscordInfoVo vo = new()
      {
        details = _details,
        largeText = "Kiesma's Game",
        state = _state,
        timer = _timer
      };
      
      SetStatus(vo);
    }
    
    public void StarterSettings()
    {
      SetStatusInfo(null, LocalizationManager.Localize("DiscordOnLoginScreen"), false);

      lastState = 0;
    }

    public void OnMenu(string username)
    {
      _username = username;
      
      SetStatusInfo(
        LocalizationManager.Localize("DiscordUsername") + " " + username,
        LocalizationManager.Localize("DiscordOnMenu"),
        false);
      
      lastState = 1;
    }
    
    public void InLobby(string username, int playerCount, int maxPlayerCount)
    {
      _username = username;
      string lobbyInfo = $" ({playerCount.ToString()} / {maxPlayerCount.ToString()})";
      
      SetStatusInfo(
        LocalizationManager.Localize("DiscordUsername") + " " + username,
        LocalizationManager.Localize("DiscordInLobby") + lobbyInfo,
        false);
      
      lastState = 2;
    }
    
    public void InGame(string username)
    {
      _username = username;
      string gameInfo = $"{LocalizationManager.Localize("DiscordInGame")} " +
                        $"({LocalizationManager.Localize("DiscordNormalGame")})";
      
      SetStatusInfo(
        LocalizationManager.Localize("DiscordUsername") + " " + username,
        gameInfo,
        true);
      
      lastState = 3;
    }
  }
}