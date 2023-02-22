using Riptide;

namespace Runtime.Contexts.Network.Services.NetworkManager
{
    public interface INetworkManagerService
    {
        Client Client { get; }
        
        void Connect(string _ip, ushort _port);
        T GetData<T>(string message) where T : new();
        Message SetData(Message message,object obj);
        void Ticker();
        void OnQuit();
    }
}
