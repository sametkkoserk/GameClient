using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Processor
{
  public class OutFromLobbyDoneProcessor : EventCommand
  {
    [Inject]
    public ILobbyModel lobbyModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      string message = vo.message;
      ushort inLobbyId = networkManager.GetData<ushort>(message);
      Debug.Log("outed message received");
      if (inLobbyId == lobbyModel.inLobbyId)
      {
        lobbyModel.LobbyReset();
        screenManagerModel.OpenPanel(SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel, LobbyKey.LobbyPanel);
      }
      else
      {
        lobbyModel.OutFromLobby(inLobbyId);
        dispatcher.Dispatch(LobbyEvent.PlayerIsOut, inLobbyId);
      }
    }
  }
}