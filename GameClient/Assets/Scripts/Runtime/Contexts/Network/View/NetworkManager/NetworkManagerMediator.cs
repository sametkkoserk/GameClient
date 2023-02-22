using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.mediation.impl;

namespace Runtime.Contexts.Network.View.NetworkManager
{
    public class NetworkManagerMediator : EventMediator
    {
        [Inject]
        public INetworkManagerService networkManager{get;set;}
        private void FixedUpdate()
        {
            networkManager.Ticker();
        }

        private void OnApplicationQuit()
        {
            networkManager.OnQuit();
        }
    }
}