using Runtime.Contexts.Lobby.Enum;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;

namespace Runtime.Contexts.Lobby.Processor
{
  public class LobbyIsClosedProcessor : EventCommand
  {
    public override void Execute()
    {
      MessageReceivedVo unnecessaryData = (MessageReceivedVo)evt.data;
      
      dispatcher.Dispatch(LobbyEvent.RefreshLobbyList);
    }
  }
}