using System;
using Discord;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.Discord.Vo;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Modules.Core.Discord.Model
{
  public class DiscordModel : IDiscordModel
  {
    public const long applicationID = 1105914769069842582;

    public const string gameLogo = "first";

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

    public void SetStatusInfo(string _details, string _largeText, string _state, bool _timer)
    {
      DiscordInfoVo vo = new()
      {
        details = _details,
        largeText = _largeText,
        state = _state,
        timer = _timer
      };
      
      SetStatus(vo);
    }
    
    public void StarterSettings()
    {
      SetStatusInfo(null, "My Game", LocalizationModel.instance.GetText(TableKey.General, "DiscordOnLoginScreen"), false);

      lastState = 0;
    }

    public void OnMenu(string username)
    {
      _username = username;
      
      SetStatusInfo(
        LocalizationModel.instance.GetText(TableKey.General, "DiscordUsername") + " " + username,
        "My Game",
        LocalizationModel.instance.GetText(TableKey.General, "DiscordOnMenu"),
        false);
      
      lastState = 1;
    }
    
    public void InLobby(string username, int playerCount, int maxPlayerCount)
    {
      _username = username;
      string lobbyInfo = $" ({playerCount.ToString()} / {maxPlayerCount.ToString()})";
      
      SetStatusInfo(
        LocalizationModel.instance.GetText(TableKey.General, "DiscordUsername") + " " + username,
        "My Game",
        LocalizationModel.instance.GetText(TableKey.General, "DiscordInLobby") + lobbyInfo,
        false);
      
      lastState = 2;
    }
    
    public void InGame(string username)
    {
      _username = username;
      string gameInfo = $"{LocalizationModel.instance.GetText(TableKey.General, "DiscordInGame")} " +
                        $"({LocalizationModel.instance.GetText(TableKey.General, "DiscordNormalGame")})";
      
      SetStatusInfo(
        LocalizationModel.instance.GetText(TableKey.General, "DiscordUsername") + " " + username,
        "My Game",
        gameInfo,
        true);
      
      lastState = 3;
    }
  }
}