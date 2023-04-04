using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Processor
{
  public class JoinedToLobbyProcessor : EventCommand
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
      Debug.Log("joined to lobby");
      JoinedToLobbyVo joinedToLobbyVo = networkManager.GetData<JoinedToLobbyVo>(message);
      // lobbyVo.lobbyId = message.GetUShort();
      // lobbyVo.lobbyName = message.GetString();
      // lobbyVo.isPrivate = message.GetBool();
      // lobbyVo.leaderId = message.GetUShort();
      // lobbyVo.playerCount = message.GetUShort();
      // lobbyVo.maxPlayerCount = message.GetUShort();
      // lobbyModel.inLobbyId = message.GetUShort();
      // lobbyVo.clients = new Dictionary<ushort, ClientVo>();
      // for (int i = 0; i < lobbyVo.playerCount; i++)
      // {
      //   ClientVo clientVo = new ClientVo();
      //   clientVo.id = message.GetUShort();
      //   clientVo.inLobbyId = message.GetUShort();
      //   //clientVo.userName = message.GetString();
      //   clientVo.colorId = message.GetUShort();
      //   lobbyVo.clients[clientVo.inLobbyId]=clientVo;
      // }

      lobbyModel.lobbyVo = joinedToLobbyVo.lobby;
      lobbyModel.clientVo = joinedToLobbyVo.clientVo;

      screenManagerModel.OpenPanel(LobbyKey.LobbyManagerPanel, SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }
  }
}