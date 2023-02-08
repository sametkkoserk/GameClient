namespace Multiplayer.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        void Connect(string _ip, ushort _port);
        void Ticker();
        void OnQuit();
    }
}
