using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using Runtime.Modules.Core.Discord.Model;

namespace Runtime.Modules.Core.Discord.View
{
  public enum DiscordBehaviourEvent
  {
    Destroy
  }
  public class DiscordBehaviourMediator : EventMediator
  {
    [Inject]
    public DiscordBehaviourView view { get; set; }
    
    [Inject]
    public IDiscordModel discordModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(DiscordBehaviourEvent.Destroy, OnDestroy);
    }

    private void OnDestroy()
    {
      Destroy(gameObject);
    }

    private void Update()
    {
      discordModel?.NetworkUpdateMethod();
    }

    private void LateUpdate()
    {
      discordModel?.NetworkLateUpdateMethod();
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(DiscordBehaviourEvent.Destroy, OnDestroy);
    }
  }
}