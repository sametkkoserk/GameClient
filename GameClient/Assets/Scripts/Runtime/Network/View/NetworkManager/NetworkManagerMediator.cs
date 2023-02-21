using Runtime.Network.Services.NetworkManager;
using strange.extensions.mediation.impl;

namespace Runtime.Network.View.NetworkManager
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