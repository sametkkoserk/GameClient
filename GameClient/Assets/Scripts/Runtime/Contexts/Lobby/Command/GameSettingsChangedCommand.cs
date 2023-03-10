using Riptide;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Command
{
  public class GameSettingsChangedCommand : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      LobbySettingsVo lobbySettingsVo = (LobbySettingsVo)evt.data;
      Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.GameSettingsChanged);
      message = networkManager.SetData(message, lobbySettingsVo);
      networkManager.Client.Send(message);
    }
  }
}