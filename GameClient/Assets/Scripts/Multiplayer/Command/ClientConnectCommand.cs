using Multiplayer.Services.NetworkManager;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

namespace Multiplayer.Command
{
    public class ClientConnectCommand : EventCommand
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        public override void Execute()
        {
            networkManager.Connect("127.0.0.1", 8084);
            
        }
    }
}