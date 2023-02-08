using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

namespace Multiplayer.Services.NetworkManager
{
    public class NetworkManagerService :  INetworkManagerService
    {
        public Client Client { get; private set; }

        [SerializeField] private string ip;
        [SerializeField] private ushort port;

        public void Connect(string _ip, ushort _port)
        {
            ip = _ip;
            port = _port;
            
            RiptideLogger.Initialize(Debug.Log,Debug.Log,Debug.LogWarning,Debug.LogError,false);
            Client = new Client();
            Client.Connect($"{ip}:{port}");
            
        }

        public void Ticker()
        {
            if (Client != null)
            {
                Client.Tick();
            }
        }

        public void OnQuit()
        {
            Client.Disconnect();
        }
    }
}
