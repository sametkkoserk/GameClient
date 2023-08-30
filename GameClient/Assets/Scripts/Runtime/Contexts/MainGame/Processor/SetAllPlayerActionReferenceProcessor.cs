using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Processor
{
  public class SetAllPlayerActionReferenceProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      
    }
  }
}