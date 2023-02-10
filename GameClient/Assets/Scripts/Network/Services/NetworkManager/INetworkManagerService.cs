using Riptide;

namespace Multiplayer.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        Client Client { get; }
        
        void Connect(string _ip, ushort _port);
        void Ticker();
        void OnQuit();
    }
}
