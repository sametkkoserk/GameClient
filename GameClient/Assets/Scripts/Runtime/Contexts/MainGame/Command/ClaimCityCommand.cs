using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class ClaimCityCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      
    }
  }
}