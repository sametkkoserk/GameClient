using Runtime.Modules.Core.Discord.Model;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.Application
{
  public class ApplicationMediator : EventMediator
  {
    [Inject]
    public ApplicationView view { get; set; }

    [Inject]
    public IDiscordModel discordModel { get; set; }
    public override void OnRegister()
    {
    }

    private void OnApplicationQuit()
    {
      discordModel?.Dispose();
    }

    public override void OnRemove()
    {
    }
  }
}