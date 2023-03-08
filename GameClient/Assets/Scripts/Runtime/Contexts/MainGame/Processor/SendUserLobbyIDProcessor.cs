using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Processor
{
  public class SendUserLobbyIDProcessor : EventCommand
  {
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    [Inject]
    public INetworkManagerService networkManager { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;

      ushort lobbyID = networkManager.GetData<ushort>(vo.message);

      mainGameModel.lobbyID = lobbyID;
    }
  }
}