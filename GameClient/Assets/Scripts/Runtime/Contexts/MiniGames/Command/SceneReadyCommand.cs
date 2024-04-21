using Riptide;
using Runtime.Contexts.Lobby.Model.LobbyModel;
using Runtime.Contexts.MiniGames.Model.MiniGamesModel;
using Runtime.Contexts.MiniGames.Vo;
using Runtime.Contexts.Network.Enum;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MiniGames.Command
{
    public class SceneReadyCommand : EventCommand
    {
        [Inject] public INetworkManagerService networkManager { get; set; }
        [Inject] public ILobbyModel lobbyModel { get; set; }


        public override void Execute()
        {
            MiniGameSceneReadyVo vo = new MiniGameSceneReadyVo()
            {
                lobbyCode = lobbyModel.lobbyVo.lobbyCode
            };
            
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.MiniGameSceneReady);
            message = networkManager.SetData(message, vo);
            networkManager.Client.Send(message);

        }
    }
}